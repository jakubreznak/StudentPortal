import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Predmet } from '../models/predmet';
import { User } from '../models/user';

@Component({
  selector: 'app-materialy',
  templateUrl: './materialy.component.html',
  styleUrls: ['./materialy.component.css']
})
export class MaterialyComponent implements OnInit {
  @Input() predmet: Predmet;

  constructor(private httpClient: HttpClient) { }
  baseUrl = environment.apiUrl;
  
  ngOnInit(): void {
  }

  onFilesSelected(evt: Event) {
    const files: FileList = (evt.target as HTMLInputElement).files;
    const formData = new FormData();
        const file = files[0];
        // IMPORTANT: 'files' must match parameter name in controller's action
        formData.append('file', file, file.name);

    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    this.httpClient
     .post(this.baseUrl + 'predmety/add-file/' + this.predmet.id, formData, { headers })
     .subscribe(result => {
        console.log(`uploaded: ${result}`)
        location.reload();
     });
  }
}