import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ActivityListComponent } from './activity-list/activity-list.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { CreateNewActivityComponent } from './create-new-activity/create-new-activity.component';
import { RegistrationComponent } from './registration/registration.component';
import { BookingsListComponent } from './bookings-list/bookings-list.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegistrationComponent },
  { path: 'activities', component: ActivityListComponent },
  { path: 'profile', component: UserProfileComponent },
  { path: 'create-activity', component: CreateNewActivityComponent },
  { path: 'user-profile', component: UserProfileComponent },
  { path: 'user-bookings', component: BookingsListComponent },
  { path: '', redirectTo: '/activities', pathMatch: 'full' }
];
