import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MaterialyComponent } from './materialy/materialy.component';
import { PredmetyListComponent } from './predmety-list/predmety-list.component';
import { PredmetDetailComponent } from './predmet-detail/predmet-detail.component';
import { SharedModule } from './modules/shared.module';
import { DiskuzeComponent } from './diskuze/diskuze.component';
import { TopicComponent } from './topic/topic.component';
import { HodnoceniComponent } from './hodnoceni/hodnoceni.component';

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
    SharedModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
