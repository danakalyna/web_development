import { Activity } from "./activity.model";

export interface Booking {
    id: string;
    bookingId: number;
    status: string;
    price: number;
    userId: string;
    activityId: number;
    activity: Activity;  // You might want to replace 'any' with a specific interface if you know the structure of `activity`
  }
  