import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  requestCount = 0;

  constructor(private spinner: NgxSpinnerService) { }

  loading() {
    this.requestCount++;
    this.spinner.show(undefined, {
      type: 'square-jelly-box',
      bdColor: 'rgba(255,255,255,0)',
      color: '#000000'
    })
  }

  hide() {
    this.requestCount--;
    if (this.requestCount <= 0){
      this.requestCount = 0;
      this.spinner.hide();
    }
  }
}
