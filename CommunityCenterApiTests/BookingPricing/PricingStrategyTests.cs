using NUnit.Framework;
using CommunityCenterApi.Models;
using CommunityCenterApi.BookingPricing;

namespace CommunityCenterApiTests.BookingPricing
{
    [TestFixture]
    public class PricingStrategyTests
    {
        [Test]
        public void StandardPricingStrategy_CalculatePrice_ReturnsOriginalPrice()
        {
            // Arrange
            var booking = new Booking { Price = 100 };
            var strategy = new StandardPricingStrategy();

            // Act
            var result = strategy.CalculatePrice(booking);

            // Assert
            Assert.AreEqual(100, result);
        }

        [Test]
        public void MemberPricingStrategy_CalculatePrice_AppliesDiscount()
        {
            // Arrange
            var booking = new Booking { Price = 100 };
            var strategy = new MemberPricingStrategy();

            // Act
            var result = strategy.CalculatePrice(booking);

            // Assert
            Assert.AreEqual(95, result);  // Expecting a 5% discount
        }
    }
}
