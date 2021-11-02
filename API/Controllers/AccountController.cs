using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.HelpClass;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using API.Entities._HelpEntities;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;
        private readonly UserManager<Student> _userManager;
        private readonly SignInManager<Student> _signInManager;
        private readonly DataContext _context;

        public AccountController(UserManager<Student> userManager, SignInManager<Student> signInManager,
        DataContext context, ITokenService tokenService)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _httpClient = new HttpClient();
        }

        [HttpPost("register")]
        public async Task<ActionResult<StudentDTO>> Register(RegisterDTO registerDTO)
        {

            if (await NameExists(registerDTO.name = RemoveAccents(registerDTO.name)))
                return BadRequest("Existuje již uživatel s tímto jménem.");

            registerDTO.upolNumber = registerDTO.upolNumber.ToUpper().Trim();
            var student = new Student
            {
                UserName = registerDTO.name.ToLower(),
                upolNumber = registerDTO.upolNumber,
                datumRegistrace = DateTime.Now,
                oborIdno = registerDTO.oborIdno,
                rocnikRegistrace = registerDTO.rocnikRegistrace
            };

            // student = await GetPredmetyFromUpol(registerDTO.upolNumber, student);
            // if (student == null)
            //     return BadRequest("Student s tímto osobním číslem neexistuje.");

            foreach (var predmet in registerDTO.predmety)
            {
                if (!(await _context.Predmets.AnyAsync(p => p.nazev == predmet.nazev && p.zkratka == predmet.zkratka && p.oborIdNum == student.oborIdno)))
                {
                    Predmet novyPredmet = new Predmet();
                    predmet.oborIdNum = student.oborIdno;
                    novyPredmet = predmet;
                    _context.Predmets.Add(novyPredmet);
                    student.predmetyStudenta.Add(novyPredmet);
                }
                else
                {
                    student.predmetyStudenta.Add(_context.Predmets.FirstOrDefault(x => x.nazev == predmet.nazev && x.zkratka == predmet.zkratka && x.oborIdNum == student.oborIdno));
                }
            }

            var result = await _userManager.CreateAsync(student, registerDTO.password);
            if (!result.Succeeded) return BadRequest("Jméno studenta obsahuje nějaký nepovolený znak.");

            var roleResult = await _userManager.AddToRoleAsync(student, "Member");
            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new StudentDTO
            {
                name = student.UserName,
                token = await _tokenService.CreateToken(student)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<StudentDTO>> Login(LoginDTO loginDTO)
        {
            var student = await _userManager.Users.FirstOrDefaultAsync(s => s.UserName == RemoveAccents(loginDTO.name.ToLower()));

            if (student == null)
                return Unauthorized("Uživatel s tímto jménem neexistuje.");

            var result = await _signInManager.CheckPasswordSignInAsync(student, loginDTO.password, false);
            if (!result.Succeeded) return Unauthorized("Špatné heslo.");

            return new StudentDTO
            {
                name = student.UserName,
                token = await _tokenService.CreateToken(student)
            };
        }

        [HttpPut("{upolNumber}/{oborId}")]
        [Authorize]
        public async Task<ActionResult<Student>> ChangeUpolNumber(string upolNumber, int oborId, [FromBody] Predmet[] predmety)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);
            student.predmetyStudenta.RemoveAll(x => true);
            
            var predmets = _context.Predmets.Include(p => p.Students).Where(p => p.Students.Contains(student));
            foreach(var predmet in predmets)
            {
                predmet.Students.RemoveAll(s => s.Id == student.Id);
            }

            if (student == null)
                return BadRequest("Student s tímto osobním číslem neexistuje.");

            if(student.upolNumber == upolNumber)
                return Ok(student);

            foreach (var predmet in predmety)
            {
                if (!(await _context.Predmets.AnyAsync(p => p.nazev == predmet.nazev && p.zkratka == predmet.zkratka && p.oborIdNum == oborId)))
                {
                    Predmet novyPredmet = new Predmet();
                    predmet.oborIdNum = oborId;
                    novyPredmet = predmet;
                    _context.Predmets.Add(novyPredmet);
                    student.predmetyStudenta.Add(novyPredmet);
                }
                else
                {
                    student.predmetyStudenta.Add(_context.Predmets.FirstOrDefault(x => x.nazev == predmet.nazev && x.zkratka == predmet.zkratka && x.oborIdNum == oborId));
                }
            }
                
            student.upolNumber = upolNumber;
            student.oborIdno = oborId;

            var result = await _userManager.UpdateAsync(student);
            if (!result.Succeeded) return BadRequest(result.Errors);

            _context.SaveChanges();

            return Ok(student);
        }

        [HttpPut("password")]
        [Authorize]
        public async Task<ActionResult<StudentDTO>> ChangePassword([FromBody] NewPassword password)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            var res = await _signInManager.CheckPasswordSignInAsync(student, password.oldPassword, false);
            if (!res.Succeeded) return BadRequest("Špatné staré heslo.");

            var result = await _userManager.ChangePasswordAsync(student, password.oldPassword, password.newPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteMyAccount()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            var predmets = _context.Predmets.Include(p => p.Students).Where(p => p.Students.Contains(student));
            foreach(var predmet in predmets)
            {
                predmet.Students.RemoveAll(s => s.Id == student.Id);
            }

            if (student == null)
                return BadRequest("Nastala chyba.");

            var result = await _userManager.DeleteAsync(student);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }

        // private async Task<Student> GetPredmetyFromUpol(string upolNumber, Student student) //nepouziva se
        // {
        //     //GET oborIdno a rocnik z UPOL API
        //     var response = await _httpClient
        //         .GetAsync("https://stagservices.upol.cz/ws/services/rest2/programy/getPlanyStudenta?osCislo=" + upolNumber + "&outputFormat=JSON");

        //     string jsonResponse = await response.Content.ReadAsStringAsync();
        //     Root data = JsonConvert.DeserializeObject<Root>(jsonResponse);
        //     if (!data.planInfo.Any()) return null;
        //     student.oborIdno = data.planInfo[0].oborIdno;
        //     student.rocnikRegistrace = ExtractNumber(data.planInfo[0].nazev);

        //     //GET predmety oboru, zkontrolovat zda uz vsechny jsou v databazi
        //     response = await _httpClient
        //         .GetAsync("https://stagservices.upol.cz/ws/services/rest2/predmety/getPredmetyByObor?oborIdno=" + student.oborIdno + "&outputFormat=JSON");
        //     jsonResponse = await response.Content.ReadAsStringAsync();
        //     RootPredmet predmety = JsonConvert.DeserializeObject<RootPredmet>(jsonResponse);

        //     foreach (var predmet in predmety.predmetOboru.Where((coordinate, index) => index % 2 == 0))
        //     {
        //         if (!(await _context.Predmets.AnyAsync(p => p.nazev == predmet.nazev && p.zkratka == predmet.zkratka && p.oborIdNum == student.oborIdno)))
        //         {
        //             Predmet novyPredmet = new Predmet();
        //             predmet.oborIdNum = student.oborIdno;
        //             novyPredmet = predmet;
        //             _context.Predmets.Add(novyPredmet);
        //         }
        //     }
        //     return student;
        // }

        private async Task<bool> NameExists(string name)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == name.ToLower());
        }

        private int ExtractNumber(string text)
        {
            int i = 0;
            while (i < text.Length)
            {
                if (Char.IsDigit(text[i]))
                {
                    return int.Parse(text[i].ToString());
                }
                i++;
            }
            return 0;
        }

        private string RemoveAccents(string username)
        {
            string result = username.Replace(" ", string.Empty);

            var normalizedString = result.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}