using CommunityCenterApi.Models;

namespace CommunityCenterApi.BookingPricing
{
    public interface IBookingPricingStrategy
    {
        double CalculatePrice(Booking booking);
    }

    public class StandardPricingStrategy : IBookingPricingStrategy
    {
        public double CalculatePrice(Booking booking)
        {
            return booking.Price;
        }
    }

    public class MemberPricingStrategy : IBookingPricingStrategy
    {
        public double CalculatePrice(Booking booking)
        {
            // підписники мають знижку
            return booking.Price * 0.95;
        }
    }
}
