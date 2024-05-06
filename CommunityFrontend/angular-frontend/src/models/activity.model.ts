import { Booking } from "./booking.model";
import { User } from "./user.model";

export interface Activity {
    id: number;
    activityId: number;
    activityName: string;
    description: string;
    userId: string;
    user?: User;
    bookings?: Booking[];
    date: Date;
    isBooked: boolean
  }