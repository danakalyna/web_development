import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Booking } from '../models/booking.model';  // Ensure you have a Booking model
import { environment } from '../environments/environment';
import { AuthService } from './authService';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private apiUrl = `${environment.apiUrl}api/bookings`;

  constructor(private http: HttpClient, private authService: AuthService) {}

  getBookings(): Observable<Booking[]> {
    return this.http.get<{ $values: Booking[] }>(this.apiUrl+"/user/"+this.authService.currentUserId).pipe(
        map(response => response.$values) // Assuming that the activities are wrapped in an object with a `$values` key
      );
  }

  book(activityId: number, date: string, price: number, userId: string){
    return this.http.post<Booking[]>(this.apiUrl, {
        date,
        price,
        userId,
        activityId,
        status: "1"
    });
  }
}
