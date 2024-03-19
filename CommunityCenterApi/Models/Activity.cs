namespace CommunityCenterApi.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        // Other properties related to the activity

        // Navigation property to User
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public DateTime Date { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}
