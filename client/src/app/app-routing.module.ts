import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { DiskuzeComponent } from './diskuze/diskuze.component';
import { AdminGuard } from './guards/admin.guard';
import { AuthGuard } from './guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { PredmetDetailComponent } from './predmet-detail/predmet-detail.component';
import { PredmetyListComponent } from './predmety-list/predmety-list.component';
import { ProfileComponent } from './profile/profile.component';
import { TopicComponent } from './topic/topic.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'predmety', component: PredmetyListComponent},
      {path: 'predmety/:id', component: PredmetDetailComponent},
      {path: 'diskuze', component: DiskuzeComponent},
      {path: 'diskuze/:topicid', component: TopicComponent},
      {path: 'predmety/:predmetid/:topicid', component: TopicComponent},
      {path: 'profile', component: ProfileComponent},
      {path: 'admin', component: AdminComponent, canActivate: [AdminGuard]}
    ]
  },
  {path: '**', component: HomeComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
