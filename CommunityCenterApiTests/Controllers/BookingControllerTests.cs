using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using CommunityCenterApi.Controllers;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;

namespace CommunityCenterApiTests.Controllers
{
    [TestFixture]
    public class BookingsControllerTests
    {
        private Mock<IBookingService> _mockBookingService;
        private BookingsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockBookingService = new Mock<IBookingService>();
            _controller = new BookingsController(_mockBookingService.Object);
        }

        [Test]
        public async Task CreateBooking_ValidBooking_ReturnsCreatedAtActionResult()
        {
            var newBooking = new Booking { BookingId = 1, UserId = Guid.NewGuid(), ActivityId = 1, Price = 100.00 };
            _mockBookingService.Setup(x => x.CreateBookingAsync(It.IsAny<Booking>()))
                .ReturnsAsync(newBooking);

            var result = await _controller.CreateBooking(newBooking);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        }

        [Test]
        public async Task GetAllBookings_ReturnsAllBookings()
        {
            var bookings = new List<Booking>
            {
                new Booking { BookingId = 1, UserId = Guid.NewGuid(), ActivityId = 1, Price = 100.00 },
                new Booking { BookingId = 2, UserId = Guid.NewGuid(), ActivityId = 2, Price = 150.00 }
            };
            _mockBookingService.Setup(x => x.GetAllBookingsAsync()).ReturnsAsync(bookings);

            var result = await _controller.GetAllBookings();

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(2, ((IEnumerable<Booking>)okResult.Value).Count());
        }

        [Test]
        public async Task GetBookingById_ExistingId_ReturnsBooking()
        {
            var booking = new Booking { BookingId = 1, UserId = Guid.NewGuid(), ActivityId = 1, Price = 100.00 };
            _mockBookingService.Setup(x => x.GetBookingByIdAsync(1)).ReturnsAsync(booking);

            var result = await _controller.GetBookingById(1);

            Assert.AreEqual(booking, result.Value);
        }

        [Test]
        public async Task UpdateBooking_ValidData_ReturnsNoContent()
        {
            var booking = new Booking { BookingId = 1, UserId = Guid.NewGuid(), ActivityId = 1, Price = 100.00 };
            _mockBookingService.Setup(x => x.UpdateBookingAsync(1, booking)).ReturnsAsync(booking);

            var result = await _controller.UpdateBooking(1, booking);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteBooking_ExistingId_ReturnsNoContent()
        {
            _mockBookingService.Setup(x => x.DeleteBookingAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteBooking(1);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetBookingPrice_ValidRequest_ReturnsPrice()
        {
            var userId = Guid.NewGuid();
            _mockBookingService.Setup(x => x.CalculatePriceAsync(1, userId)).ReturnsAsync(100.00);

            var result = await _controller.GetBookingPrice(1, userId);

            var okResult = result.Result as OkObjectResult;

            Assert.AreEqual(100.00, okResult.Value);
        }

        [Test]
        public async Task GetBookingByUserId_ValidUserId_ReturnsBookings()
        {
            var userId = Guid.NewGuid();
            var bookings = new List<BookingDto>
            {
                new BookingDto { BookingId = 1, Status = "Confirmed", Price = 100.00, UserId = userId.ToString(), Activity = new ActivityDto { ActivityId = 1, ActivityName = "Yoga", Description = "Morning Yoga", Date = DateTime.Now } }
            };

            var activity = new Activity()
            {
                ActivityId = 1,
                ActivityName = "Yoga",
                Description = "Morning Yoga",
                Date = DateTime.Now
            };

            _mockBookingService.Setup(x => x.GetAllBookingsAsync())
                .ReturnsAsync(bookings.Select(b => new Booking {
                    Status = "Confirmed", BookingId = b.BookingId, Price = b.Price, UserId = Guid.Parse(b.UserId), ActivityId = b.Activity.ActivityId, Activity = activity }).ToList());

            var result = await _controller.GetBookingByUserId(userId);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var returnedBookings = okResult.Value as IEnumerable<BookingDto>;
            Assert.AreEqual(1, returnedBookings.Count());
            Assert.AreEqual("Confirmed", returnedBookings.First().Status);
        }
    }
}
