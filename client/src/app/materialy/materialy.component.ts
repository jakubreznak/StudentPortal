import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Predmet, Soubor } from '../models/predmet';
import { Student } from '../models/student';
import { AccountService } from '../Services/account.service';
import { PredmetyService } from '../Services/predmety.service';

@Component({
  selector: 'app-materialy',
  templateUrl: './materialy.component.html',
  styleUrls: ['./materialy.component.css']
})
export class MaterialyComponent implements OnInit {
  @Input() predmet: Predmet;
  student: Student;
  baseUrl = environment.apiUrl;
  materialForm: FormGroup;

  constructor(private httpClient: HttpClient, private toastr: ToastrService, private predmetService: PredmetyService,
     private accountService: AccountService) { 
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
  }
  
  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.materialForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.maxLength(200)]),
      file: new FormControl('', Validators.required),
    })
  }

  postMaterial(){
    const formData = new FormData();

    this.materialForm.get('file').value.forEach((f) => formData.append('files', f));

    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    this.httpClient
     .post<Soubor[]>(this.baseUrl + 'predmety/add-file/' + this.predmet.id + '/' + this.materialForm.get('name').value, formData, { headers })
     .subscribe(files => {
        this.predmet.files = files;
        this.toastr.success("Materiál byl úspěšně přidán.");
        this.materialForm.reset();
      });    
  }

  onFilesSelected(evt: Event) {
    const files: FileList = (evt.target as HTMLInputElement).files;
    var selectedFiles:File[] = [];

    for (let i = 0; i < files.length; i++) {
      selectedFiles.push(files[i]);
    }

    this.materialForm.patchValue({
      file: selectedFiles
    })
  }

  deleteMaterial(predmetID, souborID){
    this.predmetService.deleteMaterial(predmetID, souborID).subscribe(soubor =>
      {
        this.toastr.success("Studijní materiál úspěšně odebrán.");
        this.predmet.files = this.predmet.files.filter(s => s.id != soubor.id);
      });
  }
}