using CommunityCenterApi.Models;

namespace CommunityCenterApi.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(Booking newBooking);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<Booking> UpdateBookingAsync(int bookingId, Booking updatedBooking);
        Task<bool> DeleteBookingAsync(int bookingId); 
        Task<double> CalculatePriceAsync(int bookingId, Guid userId);
    }
}
