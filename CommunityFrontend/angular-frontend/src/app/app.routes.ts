import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ActivityListComponent } from './activity-list/activity-list.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { CreateNewActivityComponent } from './create-new-activity/create-new-activity.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'activities', component: ActivityListComponent },
  { path: 'profile', component: UserProfileComponent },
  { path: 'create-activity', component: CreateNewActivityComponent },
  { path: 'user-profile', component: UserProfileComponent },
  { path: '', redirectTo: '/activities', pathMatch: 'full' }
];
