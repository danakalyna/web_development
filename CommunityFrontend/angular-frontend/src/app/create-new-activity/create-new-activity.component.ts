import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivityService } from '../../services/activityService';
import { AuthService } from '../../services/authService';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-new-activity',
  templateUrl: './create-new-activity.component.html',
  styleUrls: ['./create-new-activity.component.scss']
})
export class CreateNewActivityComponent {
  activityForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private activityService: ActivityService,
    private authService: AuthService,
    private toastr: ToastrService
  ) {
    this.activityForm = this.formBuilder.group({
      activityName: ['', Validators.required],
      description: [''],
      date: ['', Validators.required] // Changed from new Date() to '' to avoid automatic date filling
    });
  }

  createActivity() {
    if (this.activityForm.valid) {
      const { activityName, description, date } = this.activityForm.value;
      const userId = this.authService.currentUserId;
      this.activityService.createActivity(activityName, description, date, userId).subscribe({
        next: (activity) => {
          console.log('Activity created:', activity);
          this.toastr.success('Activity created successfully');
          // Optionally redirect or clear the form here
          this.activityForm.reset();
        },
        error: (error) => {
          console.error('Failed to create activity:', error);
          this.toastr.error('Failed to create activity');
        }
      });
    }
  }
}
