using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommunityCenterApi.Controllers;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommunityCenterApiTests.Controllers
{
    [TestFixture]
    public class BookingsControllerTests
    {
        private Mock<IBookingService> _mockBookingService;
        private BookingsController _controller;
        private List<Booking> _bookings;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockBookingService = new Mock<IBookingService>();
            _controller = new BookingsController(_mockBookingService.Object);
            _bookings = new List<Booking>
            {
                new Booking { BookingId = 1, UserId = Guid.NewGuid(), Price = 100 },
                new Booking { BookingId = 2, UserId = Guid.NewGuid(), Price = 150 }
            };
        }

        [Test]
        public async Task GetAllBookings_ReturnsAllBookings()
        {
            _mockBookingService.Setup(service => service.GetAllBookingsAsync())
                               .ReturnsAsync(_bookings);

            var result = await _controller.GetAllBookings();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnBookings = okResult.Value as List<Booking>;
            Assert.IsNotNull(returnBookings);
            Assert.AreEqual(2, returnBookings.Count);
        }

        [Test]
        public async Task CreateBooking_ReturnsCreatedAtAction_WhenValid()
        {
            var newBooking = new Booking { BookingId = 3, UserId = Guid.NewGuid(), Price = 200 };
            _mockBookingService.Setup(service => service.CreateBookingAsync(It.IsAny<Booking>()))
                               .ReturnsAsync(newBooking);

            var result = await _controller.CreateBooking(newBooking);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            var createdBooking = createdAtActionResult.Value as Booking;
            Assert.IsNotNull(createdBooking);
            Assert.AreEqual(200, createdBooking.Price);
        }


        [Test]
        public async Task UpdateBooking_ReturnsNoContent_WhenBookingIsUpdated()
        {
            var bookingId = 1;
            var updatedBooking = new Booking { BookingId = bookingId, UserId = Guid.NewGuid(), Price = 250 };
            _mockBookingService.Setup(service => service.UpdateBookingAsync(bookingId, It.IsAny<Booking>()))
                               .ReturnsAsync(updatedBooking);

            var result = await _controller.UpdateBooking(bookingId, updatedBooking);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetBookingById_ReturnsNotFound_WhenBookingDoesNotExist()
        {
            var bookingId = 99;
            _mockBookingService.Setup(service => service.GetBookingByIdAsync(bookingId))
                               .ReturnsAsync((Booking)null);

            var result = await _controller.GetBookingById(bookingId);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}
