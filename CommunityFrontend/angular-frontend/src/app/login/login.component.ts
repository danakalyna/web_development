import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router'; // Import Router
import { AuthService } from '../../services/authService'; // Make sure to implement this service
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService  // Inject ToastrService here
  ) {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  login() {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      this.authService.login(username, password).subscribe({
        next: (response) => {
          console.log('Login successful!', response);
          this.toastr.success('Login successful!'); // Display success toast
          this.router.navigate(['/activities']); // Navigate to the activities page
        },
        error: (error) => {
          console.error('Login failed', error);
          this.toastr.error('Login failed. Please check your username and password.'); // Display error toast
        }
      });
    } else {
      console.error('Form is not valid');
      this.toastr.error('Please fill in all required fields correctly.'); // Display error toast when form is invalid
    }
  }

  navigateToRegister() {
    this.router.navigate(['/register']);
  }
}
