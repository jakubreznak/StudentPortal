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

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
            _httpClient = new HttpClient();
        }

        [HttpPost("register")]
        public async Task<ActionResult<StudentDTO>> Register(RegisterDTO registerDTO)
        {
            using var hmac = new HMACSHA512();

            if (await NameExists(registerDTO.name))
                return BadRequest("Existuje již uživatel s tímto jménem.");

            var student = new Student
            {
                name = registerDTO.name.ToLower(),
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password)),
                passwordSalt = hmac.Key,
                upolNumber = registerDTO.upolNumber,
                datumRegistrace = DateTime.Now
            };

            //GET oborIdno a rocnik z UPOL API
            var response = await _httpClient
                .GetAsync("https://stag-ws.upol.cz/ws/services/rest2/programy/getPlanyStudenta?osCislo=" + registerDTO.upolNumber + "&outputFormat=JSON");
            
            string jsonResponse = await response.Content.ReadAsStringAsync();
            Root data = JsonConvert.DeserializeObject<Root>(jsonResponse);
            int oborIdno = data.planInfo[0].oborIdno;
            int rocnik = ExtractNumber(data.planInfo[0].nazev);

            student.oborIdno = oborIdno;
            student.rocnikRegistrace = rocnik;

            //GET predmety oboru, zkontrolovat zda uz vsechny jsou v databazi
            response = await _httpClient
                .GetAsync("https://stag-ws.upol.cz/ws/services/rest2/predmety/getPredmetyByObor?oborIdno=" + oborIdno + "&outputFormat=JSON");
            jsonResponse = await response.Content.ReadAsStringAsync();
            RootPredmet predmety = JsonConvert.DeserializeObject<RootPredmet>(jsonResponse);

            foreach(var predmet in predmety.predmetOboru.Where(( coordinate, index ) => index % 2 == 0 ))
            {
                if(!(await _context.Predmets.AnyAsync(p => p.nazev == predmet.nazev && p.zkratka == predmet.zkratka && p.oborIdNum == oborIdno)))
                {   
                    predmet.oborIdNum = oborIdno;
                    _context.Predmets.Add(predmet);
                }
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return new StudentDTO
            {
                name = student.name,
                token = _tokenService.CreateToken(student)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<StudentDTO>> Login(LoginDTO loginDTO)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.name == loginDTO.name);

            if (student == null)
                return Unauthorized("Uživatel s tímto jménem neexistuje.");

            using var hmac = new HMACSHA512(student.passwordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != student.passwordHash[i])
                    return Unauthorized("Špatné heslo.");
            }

            return new StudentDTO
            {
                name = student.name,
                token = _tokenService.CreateToken(student)
            };
        }

        private async Task<bool> NameExists(string name)
        {
            return await _context.Students.AnyAsync(x => x.name == name.ToLower());
        }

        private int ExtractNumber(string text)
        {
            int i = 0;
            while(i < text.Length)
            {
                if(Char.IsDigit(text[i]))
                {
                    return int.Parse(text[i].ToString());
                }
                i++;
            }
            return 0;
        }
    }
}