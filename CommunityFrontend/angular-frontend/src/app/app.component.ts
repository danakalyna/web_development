import { Component } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms'; 

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'] // Corrected 'styleUrl' to 'styleUrls'
})
export class AppComponent {
  title = 'angular-frontend';
  myFormGroup = new FormGroup({ // Example FormGroup
    myControl: new FormControl('')
  });
}[]
