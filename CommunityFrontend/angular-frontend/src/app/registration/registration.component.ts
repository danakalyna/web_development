import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/authService';
import { Router } from '@angular/router'; // Import Router
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent {
  registrationForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder, 
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService  // Inject ToastrService here
  ) {
    this.registrationForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
      password: ['', [Validators.required, Validators.maxLength(255)]],
      dateOfBirth: ['', Validators.required],
      isMember: [false, Validators.required]
    });
  }

  register() {
    if (this.registrationForm.valid) {
      const { firstName, lastName, email, password, dateOfBirth, isMember } = this.registrationForm.value;
      
      this.authService.register(firstName, lastName, email, password, dateOfBirth, isMember).subscribe({
        next: (user) => {
          console.log('Registration successful', user);
          this.toastr.success('Registration successful! Welcome!');
          this.registrationForm.reset();
          this.router.navigate(['/activities']);  // Redirect to activities page or any other route
        },
        error: (error) => {
          console.error('Registration failed', error);
          this.toastr.error('Registration failed. Please try again.');
        }
      });
    } else {
      console.log('Form is not valid');
      this.toastr.error('Please fill in all required fields correctly.');
    }
  }
}
