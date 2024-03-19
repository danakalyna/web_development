import { Component, OnInit } from '@angular/core';
import { Activity } from '../../models/activity.model';
import { ActivityService } from '../../services/activityService';

@Component({
  selector: 'app-activity-list',
  templateUrl: './activity-list.component.html',
  styleUrls: ['./activity-list.component.scss']
})
export class ActivityListComponent implements OnInit {
  activities: Activity[] = [];

  constructor(private activityService: ActivityService) {}

  ngOnInit(): void {
    this.loadActivities();
  }

  loadActivities(): void {
    this.activityService.getActivities().subscribe({
      next: (activities) => {
        this.activities = activities; 
      },
      error: (error) => {
        console.error('Error fetching activities:', error);
      }
    });
  }
}
