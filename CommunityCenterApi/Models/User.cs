using System.ComponentModel.DataAnnotations;

namespace CommunityCenterApi.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        // Additional fields here, like address, phone number, etc.

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public bool IsMember { get; set; }

        // Use this if you want to store hashed passwords (not plain text!)
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
