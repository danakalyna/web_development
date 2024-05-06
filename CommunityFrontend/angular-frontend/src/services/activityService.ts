import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Activity } from '../models/activity.model';
import { environment } from '../environments/environment';
import { AuthService } from './authService';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {
  private apiUrl = `${environment.apiUrl}api/activities`;

  constructor(private http: HttpClient, private authService: AuthService) {}

  // Get a single activity by its ID
  getActivityById(id: number): Observable<Activity> {
    return this.http.get<Activity>(`${this.apiUrl}/${id}`);
  }
  convertDate(dateString: string) {
    
    const datetime = new Date(`${dateString}`);
    return datetime.toISOString(); // Converts to a string in simplified extended ISO format (ISO 8601)
  }
  // Create a new activity activityName, description, date
  createActivity(activityName: string, description: string, date: string, userId: string): Observable<Activity> {
    return this.http.post<Activity>(this.apiUrl, {
      activityName,
      description,
      date: this.convertDate(date),
      userId
    });
  }

  // Update an existing activity
  updateActivity(activity: Activity): Observable<Activity> {
    return this.http.put<Activity>(`${this.apiUrl}/${activity.id}`, activity);
  }

  // Delete an activity by its ID
  deleteActivity(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }  
  
  getActivities(): Observable<Activity[]> {
    return this.http.get<{ $values: Activity[] }>(this.apiUrl+"/GetAllActivitiesOrderedByDate/" + this.authService.currentUserId).pipe(
      map(response => response.$values) // Assuming that the activities are wrapped in an object with a `$values` key
    );
  }
}
