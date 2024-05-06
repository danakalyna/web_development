import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms'
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RouterModule } from '@angular/router';
import { routes } from './app.routes'
import { CreateNewActivityComponent } from './create-new-activity/create-new-activity.component';
import { ActivityListComponent } from './activity-list/activity-list.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from '../services/JwtInterceptor';
import { RegistrationComponent } from './registration/registration.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // Required by Toastr
import { ToastrModule } from 'ngx-toastr';
import { BookingsListComponent } from './bookings-list/bookings-list.component';

// Import other components and services as needed

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    CreateNewActivityComponent,
    ActivityListComponent,
    UserProfileComponent,
    RegistrationComponent,
    BookingsListComponent
    // ... any other components you have
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule, // This is required for your forms
    HttpClientModule, // This is typically required for making HTTP requests
    RouterModule.forRoot(routes),
    FormsModule,
    BrowserAnimationsModule, // Toastr requires animations module
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    })
    // ... any other modules you need
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
