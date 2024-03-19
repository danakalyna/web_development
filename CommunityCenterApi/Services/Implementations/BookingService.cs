using CommunityCenterApi.BookingPricing;
using CommunityCenterApi.DB;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityCenterApi.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService userService;

        public BookingService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            this.userService = userService;
        }

        public async Task<Booking> CreateBookingAsync(Booking newBooking)
        {
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();
            return newBooking;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Activity)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Activity)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<Booking> UpdateBookingAsync(int bookingId, Booking updatedBooking)
        {
            var existingBooking = await _context.Bookings.FindAsync(bookingId);
            if (existingBooking == null)
            {
                return null;
            }

            // Update properties
            existingBooking.Status = updatedBooking.Status;
            existingBooking.UserId = updatedBooking.UserId;
            existingBooking.ActivityId = updatedBooking.ActivityId;
            // ... other properties as needed

            _context.Bookings.Update(existingBooking);
            await _context.SaveChangesAsync();
            return existingBooking;
        }

        public async Task<bool> DeleteBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return false;
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<double> CalculatePriceAsync(int bookingId, Guid userId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                throw new ArgumentException("Booking not found", nameof(bookingId));
            }
            
            // Навігатор вирішує який метод ціни обрати для користувача
            IBookingPricingStrategy pricingStrategy = await SelectPriceStrategy(userId);

            return pricingStrategy.CalculatePrice(booking);
        }

        private async Task<IBookingPricingStrategy> SelectPriceStrategy(Guid userId)
        {
            var user = await userService.GetUserByIdAsync(userId);

            IBookingPricingStrategy pricingStrategy;
            // Якщо користувач є підписником сайту то він отримує знижку
            if (user.IsMember)
            {
                pricingStrategy = new MemberPricingStrategy();
            }
            else
            {
                pricingStrategy = new StandardPricingStrategy();
            }
            return pricingStrategy;
        }
    }
}
