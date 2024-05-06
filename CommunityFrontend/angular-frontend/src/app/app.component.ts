import { Component } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms'; 
import { AuthService } from '../services/authService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'] // Corrected 'styleUrl' to 'styleUrls'
})
export class AppComponent {
  
  currentUsername: string = '';
  
  constructor(private authService: AuthService) {}

  title = 'angular-frontend';
  myFormGroup = new FormGroup({ // Example FormGroup
    myControl: new FormControl('')
  });

}[]
