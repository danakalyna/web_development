import { Component, OnInit } from '@angular/core';
import { BookingService } from '../../services/bookingService';
import { Booking } from '../../models/booking.model';

@Component({
  selector: 'app-bookings-list',
  templateUrl: './bookings-list.component.html',
  styleUrls: ['./bookings-list.component.scss']
})
export class BookingsListComponent implements OnInit {
  bookings: Booking[] = [];

  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.bookingService.getBookings().subscribe({
      next: (bookings) => {
        console.log('Bookings received:', bookings);  // Add this line
        this.bookings = bookings;
      },
      error: (error) => {
        console.error('Error fetching bookings:', error);
      }
    });
  }
  
}
