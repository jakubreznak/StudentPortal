import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MaterialyComponent } from './materialy/materialy.component';
import { PredmetyListComponent } from './predmety-list/predmety-list.component';
import { PredmetDetailComponent } from './predmet-detail/predmet-detail.component';
import { SharedModule } from './modules/shared.module';
import { DiskuzeComponent } from './diskuze/diskuze.component';
import { TopicComponent } from './topic/topic.component';
import { HodnoceniComponent } from './hodnoceni/hodnoceni.component';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './interceptors/loading.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MaterialyComponent,
    PredmetyListComponent,
    PredmetDetailComponent,
    DiskuzeComponent,
    TopicComponent,
    HodnoceniComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    NgxSpinnerModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    })
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
