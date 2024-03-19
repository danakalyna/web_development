import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RouterModule } from '@angular/router';
import { routes } from './app.routes'
import { CreateNewActivityComponent } from './create-new-activity/create-new-activity.component';
import { ActivityListComponent } from './activity-list/activity-list.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from '../services/JwtInterceptor';

// Import other components and services as needed

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    CreateNewActivityComponent,
    ActivityListComponent,
    UserProfileComponent
    // ... any other components you have
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule, // This is required for your forms
    HttpClientModule, // This is typically required for making HTTP requests
    RouterModule.forRoot(routes),
    // ... any other modules you need
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
