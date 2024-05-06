import { Component, OnInit } from '@angular/core';
import { Activity } from '../../models/activity.model';
import { ActivityService } from '../../services/activityService';
import { BookingService } from '../../services/bookingService';
import { ToastrService } from 'ngx-toastr'; // Ensure ToastrService is imported for user feedback
import { AuthService } from '../../services/authService';

@Component({
  selector: 'app-activity-list',
  templateUrl: './activity-list.component.html',
  styleUrls: ['./activity-list.component.scss']
})
export class ActivityListComponent implements OnInit {
  activities: Activity[] = [];
  filteredActivities: Activity[] = [];
  searchTerm: string = '';
  isLoading: boolean = false;
  currentPage: number = 1;
  pageSize: number = 10;  // Adjust the page size as necessary

  constructor(
    private activityService: ActivityService, 
    private bookingService: BookingService,
    private toastr: ToastrService,  // Inject Toastr for notifications
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadActivities();
  }

  loadActivities(): void {
    this.isLoading = true;
    this.activityService.getActivities().subscribe({
      next: (activities) => {
        this.activities = activities;
        this.filteredActivities = this.activities.slice(0, this.pageSize);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching activities:', error);
        this.isLoading = false;
        this.toastr.error('Failed to load activities.');
      }
    });
  }

  searchActivities(): void {
    this.currentPage = 1; // Reset to first page on search
    if (!this.searchTerm) {
      this.filteredActivities = this.activities.slice(0, this.pageSize);
    } else {
      this.filteredActivities = this.activities.filter(activity =>
        activity.activityName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        activity.description.toLowerCase().includes(this.searchTerm.toLowerCase())
      ).slice(0, this.pageSize);
    }
  }

  bookActivity(activity: Activity): void {
    if (!activity.isBooked) {
      const date = new Date().toISOString();
      this.bookingService.book(activity.activityId, date, 30, this.authService.currentUserId).subscribe({
        next: () => {
          activity.isBooked = true;  // Update local state
          this.toastr.success('Booking successful!');
        },
        error: (error) => {
          console.error('Booking failed:', error);
          this.toastr.error('Booking failed. Please try again.');
        }
      });
    }
  }
  

  onPageChange(page: number): void {
    this.currentPage = page;
    const startIndex = (this.currentPage - 1) * this.pageSize;
    this.filteredActivities = this.activities.slice(startIndex, startIndex + this.pageSize);
  }
}
