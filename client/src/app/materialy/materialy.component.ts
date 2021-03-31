import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
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

  constructor(private httpClient: HttpClient, private toastr: ToastrService, private predmetService: PredmetyService,
     private accountService: AccountService) { 
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
  }
  
  ngOnInit(): void {
  }

  onFilesSelected(evt: Event) {
    const files: FileList = (evt.target as HTMLInputElement).files;
    const formData = new FormData();
    const file = files[0];
    formData.append('file', file, file.name);

    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    this.httpClient
     .post<Soubor[]>(this.baseUrl + 'predmety/add-file/' + this.predmet.id, formData, { headers })
     .subscribe(files => {
        this.predmet.files = files;
        this.toastr.success("Materiál přidán.");
     });
  }

  deleteMaterial(predmetID, souborID){
    this.predmetService.deleteMaterial(predmetID, souborID).subscribe(soubor =>
      {
        this.toastr.success("Studijní materiál úspěšně odebrán.");
        this.predmet.files = this.predmet.files.filter(s => s.id != soubor.id);
      }, error => {
        this.toastr.error(error.error);
      });
  }
}