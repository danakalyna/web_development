using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using CommunityCenterApi.DB;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Implementations;
using CommunityCenterApi.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;
using Moq;

namespace CommunityCenterApiTests.Services
{
    [TestFixture]
    public class BookingServiceTests
    {
        private ApplicationDbContext _context;
        private BookingService _service;
        private Mock<IUserService> _mockUserService;  // Assuming you will add necessary using statement for Mock

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestBookingDb").Options;

            _context = new ApplicationDbContext(options);
            _mockUserService = new Mock<IUserService>();
            _service = new BookingService(_context, _mockUserService.Object);

            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task CreateBookingAsync_WhenCalled_AddsBookingToDatabase()
        {
            var newBooking = new Booking { ActivityId = 1, UserId = Guid.NewGuid(), Status = "Pending" };

            var result = await _service.CreateBookingAsync(newBooking);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, _context.Bookings.Count());
            Assert.AreEqual("Pending", _context.Bookings.First().Status);
        }

        [Test]
        public async Task UpdateBookingAsync_ExistingBooking_UpdatesFields()
        {
            var booking = new Booking { BookingId = 1, Status = "Pending", ActivityId = 1, UserId = Guid.NewGuid() };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            booking.Status = "Confirmed";
            var result = await _service.UpdateBookingAsync(1, booking);

            Assert.IsNotNull(result);
            Assert.AreEqual("Confirmed", result.Status);
        }

        [Test]
        public async Task DeleteBookingAsync_ExistingBooking_DeletesBooking()
        {
            var booking = new Booking { BookingId = 1, Status = "Pending", ActivityId = 1, UserId = Guid.NewGuid() };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            var success = await _service.DeleteBookingAsync(1);

            Assert.IsTrue(success);
            Assert.AreEqual(0, _context.Bookings.Count());
        }

        [Test]
        public async Task CalculatePriceAsync_UsesCorrectPricingStrategy()
        {
            var user = new User { UserId = Guid.NewGuid(), IsMember = true };
            var booking = new Booking { BookingId = 1, UserId = user.UserId, ActivityId = 1, Status = "Pending", Price = 100 };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            _mockUserService.Setup(x => x.GetUserByIdAsync(user.UserId)).ReturnsAsync(user);
            // Assuming pricing strategies are correctly mocked or implemented
            // You may want to mock the pricing strategy behavior if it's complex or external

            var price = await _service.CalculatePriceAsync(1, user.UserId);

            // Validate the price based on expected strategy
            // This assumes you know what the member pricing should return
            // For example, let's assume members get a 20% discount and standard price is 100
            Assert.AreEqual(95, price);  // Assuming the calculated price should be 95 for members
        }
        [Test]
        public async Task CalculatePriceAsync_UsesCorrectPricingStrategyNonMember()
        {
            var user = new User { UserId = Guid.NewGuid(), IsMember = false };
            var booking = new Booking { BookingId = 1, UserId = user.UserId, ActivityId = 1, Status = "Pending", Price = 100 };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            _mockUserService.Setup(x => x.GetUserByIdAsync(user.UserId)).ReturnsAsync(user);
            // Assuming pricing strategies are correctly mocked or implemented
            // You may want to mock the pricing strategy behavior if it's complex or external

            var price = await _service.CalculatePriceAsync(1, user.UserId);

            // Validate the price based on expected strategy
            // This assumes you know what the member pricing should return
            // For example, let's assume members get a 20% discount and standard price is 100
            Assert.AreEqual(100, price);  // Assuming the calculated price should be 95 for members
        }
    }
}
