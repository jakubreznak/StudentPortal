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
import { AdminComponent } from './admin/admin.component';
import { RoleDirective } from './directives/role.directive';
import { StudentiAdminComponent } from './admin/studenti-admin/studenti-admin.component';
import { DiskuzeAdminComponent } from './admin/diskuze-admin/diskuze-admin.component';
import { KomentareAdminComponent } from './admin/komentare-admin/komentare-admin.component';
import { HodnoceniAdminComponent } from './admin/hodnoceni-admin/hodnoceni-admin.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MaterialyAdminComponent } from './admin/materialy-admin/materialy-admin.component';

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
    HodnoceniComponent,
    AdminComponent,
    RoleDirective,
    StudentiAdminComponent,
    DiskuzeAdminComponent,
    KomentareAdminComponent,
    HodnoceniAdminComponent,
    MaterialyAdminComponent
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
    TabsModule.forRoot(),
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
