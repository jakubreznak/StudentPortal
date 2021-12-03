import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Hodnoceni, Predmet } from '../models/predmet';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
import { Student } from '../models/student';
import { AccountService } from '../Services/account.service';
import { Pagination } from '../models/helpModels/pagination';
import { getPaginatedResult, getPaginationHeaders } from '../Services/paginationHelper';
import { ActivatedRoute } from '@angular/router';
import { HodnoceniParams } from '../models/helpModels/hodnoceniParams';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-hodnoceni',
  templateUrl: './hodnoceni.component.html',
  styleUrls: ['./hodnoceni.component.scss']
})
export class HodnoceniComponent implements OnInit {
  @Input() predmet: Predmet;
  baseUrl = environment.apiUrl;
  hodnoceni: Hodnoceni[];
  cislo: number;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json'
    })
  };
  cisla: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
  hodnoceniForm: FormGroup;
  student: Student;
  idIsEditing: number;
  textIsEditing: string;
  editForm: FormGroup;
  pagination: Pagination;
  hodnoceniParams: HodnoceniParams = new HodnoceniParams();
  pagedRatings: Hodnoceni[];
  predmetId = Number(this.route.snapshot.paramMap.get('id'));
  hodnoceniLiked: number[] = [];
  showRating: number;
  modalRefRating?: BsModalRef;

  constructor(private http: HttpClient, private toastr: ToastrService, private accountService: AccountService,
    private route: ActivatedRoute, private modalService: BsModalService) { 
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
  }

  ngOnInit(): void {
    this.loadHodnoceni();
    this.initializeForm();
  }

  initializeForm() {
    this.hodnoceniForm = new FormGroup({
      text: new FormControl(''),
      cislo: new FormControl('', Validators.required)
    })
  }

  initializeEditForm() {
    this.editForm = new FormGroup({
      text: new FormControl('')
    })
  }

  openModalRating(template: TemplateRef<any>) {
    this.modalRefRating = this.modalService.show(template, {class: 'modal-sm'});
  }

  decline(): void {
    this.modalRefRating?.hide();
  }

  validateBeforeSubmit(){
    if(this.hodnoceniForm.valid){
      this.rate();
    }else{
      this.hodnoceniForm.markAllAsTouched();
    }
  }

  rate(){
    this.http.post<Hodnoceni[]>(this.baseUrl + 'hodnoceni/' + this.predmetId + '/' + this.hodnoceniForm.value.cislo,
    JSON.stringify(this.hodnoceniForm.value.text || ""), this.httpOptions).subscribe(hodnoceni =>
      {
        this.pagination.currentPage = 1;
        this.loadHodnoceni();
        this.toastr.success("Hodnocení přidáno.");
        this.hodnoceniForm.reset();
      });    
  }

  loadHodnoceni(){
    let params = getPaginationHeaders(this.hodnoceniParams.pageNumber, this.hodnoceniParams.pageSize);

    params = params.append('OrderBy',  this.hodnoceniParams.orderBy);

    getPaginatedResult<Hodnoceni[]>(this.baseUrl + 'hodnoceni/' + this.predmetId, params, this.http)
      .subscribe(response =>    
        {
          this.hodnoceni = response.result;
          this.pagination = response.pagination;

          this.accountService.getCurrentUserId().subscribe(id =>
            {
              let hodnocenis: number[] = [];
              response.result.forEach(function (hodnoceni){
                if(hodnoceni.studentsLikedBy.some(x => x.studentId == id))
                {
                  hodnocenis.push(hodnoceni.id);
                }
              })
              this.hodnoceniLiked = hodnocenis;
            })          
        });

      this.http.get<number>(this.baseUrl + 'hodnoceni/cislo/' + this.predmetId).subscribe(response =>
        {
          this.cislo = response;
        });
  }

  likeOrRemoveLike(materialId: number){
    if(this.hodnoceniLiked.includes(materialId)){
      this.removeLikeMaterial(materialId);
    }
    else if(!this.hodnoceniLiked.includes(materialId))
    {
      this.likeMaterial(materialId);
    }
  }

  likeMaterial(hodnoceniId: number){
    this.http.post<number>(this.baseUrl + 'like/hodnoceni', hodnoceniId).subscribe(response =>
    {
      this.toastr.success("Líbí se vám daný materiál.")
      this.loadHodnoceni();
    });
  }

  removeLikeMaterial(hodnoceniId: number){
    this.http.delete(this.baseUrl + 'like/hodnoceni/' + hodnoceniId).subscribe(response =>
    {
      this.toastr.success("Materiál se vám již nelíbí.")
      this.loadHodnoceni();
    });
  }

  pageChanged(event: any){
    this.hodnoceniParams.pageNumber = event.page;
    this.loadHodnoceni();
    window.scrollTo(0, 0);
  }

  deleteRating(hodnoceniID){
    this.http.delete<Hodnoceni>(this.baseUrl + 'hodnoceni/' + this.predmetId + '/' + hodnoceniID).subscribe(hodnoceni =>
      {
        this.pagination.currentPage = 1;
        this.loadHodnoceni();      
        this.toastr.success("Hodnocení bylo úspěšně odebráno.");
        this.modalRefRating?.hide();
      }
      );
  }

  getCislo(hodnoceni: Hodnoceni[]){
    let hodnota = 0;
    for(let i = 0; i < hodnoceni.length; i++){
      hodnota += hodnoceni[i].rating;
    }
    if(hodnoceni.length > 0){
      return Math.round(hodnota / hodnoceni.length);
    } else{
      return 0;
    }
  }

  editRating(hodnoceniID, hodnoceniText){
    this.textIsEditing = hodnoceniText;
    this.idIsEditing = hodnoceniID;
    this.initializeEditForm();
  }

  saveEdit(hodnoceniID){
    this.http.put<Hodnoceni[]>(this.baseUrl + 'hodnoceni/' + this.predmetId + '/' + hodnoceniID,
    JSON.stringify(this.editForm.value.text || ""), this.httpOptions).subscribe(hodnoceni =>
    {
      this.loadHodnoceni();
      this.toastr.success("Hodnocení bylo úspěšně upraveno.");
      this.cancelEdit();
    });
  }

  cancelEdit()
  {
    this.idIsEditing = NaN;
  }

}
