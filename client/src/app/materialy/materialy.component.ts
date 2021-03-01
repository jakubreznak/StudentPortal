import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { Predmet, Soubor } from '../models/predmet';

@Component({
  selector: 'app-materialy',
  templateUrl: './materialy.component.html',
  styleUrls: ['./materialy.component.css']
})
export class MaterialyComponent implements OnInit {
  @Input() predmet: Predmet;

  constructor(private httpClient: HttpClient, private toastr: ToastrService) { }
  baseUrl = environment.apiUrl;
  
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
}