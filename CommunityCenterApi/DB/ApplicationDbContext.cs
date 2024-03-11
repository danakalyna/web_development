using CommunityCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityCenterApi.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and any configuration

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserPermissions)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Activities)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            // Relationship between Booking and User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction); 

            // Relationship between Booking and Activity
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Activity)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.ActivityId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
