using System;

namespace CommunityCenterApi.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }

        // Foreign key and navigation property to User
        public Guid UserId { get; set; }
        public User? User { get; set; }

        // Foreign key and navigation property to Activity
        public int ActivityId { get; set; }
        public Activity? Activity { get; set; }
    }
}
