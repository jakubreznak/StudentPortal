import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MaterialParams } from '../models/helpModels/materialParams';
import { Pagination } from '../models/helpModels/pagination';
import { Predmet, Soubor } from '../models/predmet';
import { Student } from '../models/student';
import { AccountService } from '../Services/account.service';
import { getPaginatedResult, getPaginationHeaders } from '../Services/paginationHelper';
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
  pagination: Pagination;
  materialParams = new MaterialParams();
  pagedFiles: Soubor[];
  predmetId = Number(this.route.snapshot.paramMap.get('id'));
  allFilesCount: number;
  filtersToggle: boolean = false;
  filtersToggleText: string = 'Filtry';


  constructor(private httpClient: HttpClient, private toastr: ToastrService, private predmetService: PredmetyService,
     private accountService: AccountService, private route: ActivatedRoute) { 
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
  }
  
  ngOnInit(): void {
    this.initializeForm();
    if(this.predmetId != null){
      this.loadMaterials();
    }    
  }

  loadMaterials() {
    let params = getPaginationHeaders(this.materialParams.pageNumber, this.materialParams.pageSize);

    params = params.append('Nazev', this.materialParams.nazev);
    params = params.append('Typ', this.materialParams.typ);

    getPaginatedResult<Soubor[]>(this.baseUrl + 'predmety/getbyid/' + this.predmetId, params, this.httpClient)
      .subscribe(response =>    
        {
          this.pagedFiles = response.result;
          this.pagination = response.pagination;          
        });
  }

  filterToggle(){
    this.filtersToggle = !this.filtersToggle;
    this.filtersToggleText = this.filtersToggle ? 'Skrýt filtry' : 'Filtry';
  }

  pageChanged(event: any){
    this.materialParams.pageNumber = event.page;
    this.loadMaterials();
    window.scrollTo(0, 0);
  }

  resetFilters(){
    this.materialParams = new MaterialParams();
    this.pagination.currentPage = 1;
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
     .post<Soubor[]>(this.baseUrl + 'predmety/add-file/' + this.predmetId + '/' + this.materialForm.get('name').value, formData, { headers })
     .subscribe(files => {
      this.pagination.currentPage = 1;
      this.loadMaterials();
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

  deleteMaterial(souborID){
    this.predmetService.deleteMaterial(this.predmetId, souborID).subscribe(soubor =>
      {
        this.toastr.success("Studijní materiál úspěšně odebrán.");
        this.pagination.currentPage = 1;
        this.loadMaterials();
      });
  }
}