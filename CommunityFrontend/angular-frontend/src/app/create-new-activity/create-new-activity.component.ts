import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivityService } from '../../services/activityService';


@Component({
  selector: 'app-create-new-activity',
  templateUrl: './create-new-activity.component.html',
  styleUrls: ['./create-new-activity.component.scss']
})
export class CreateNewActivityComponent {
  activityForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private activityService: ActivityService
  ) {
    // Initialize the form
    this.activityForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: [''],
      startDate: [new Date(), Validators.required]
      // ...other form controls as needed
    });
  }

  // Logic to create a new activity
  createActivity() {
  }
}
