using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityCenterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            if (booking == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBooking = await _bookingService.CreateBookingAsync(booking);
            return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.BookingId }, createdBooking);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return booking;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedBooking = await _bookingService.UpdateBookingAsync(id, booking);
            if (updatedBooking == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var success = await _bookingService.DeleteBookingAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}/{userId}/price")]
        public async Task<ActionResult<double>> GetBookingPrice(int id, Guid userId)
        {
            try
            {
                var price = await _bookingService.CalculatePriceAsync(id, userId);
                return Ok(price);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                // Consider logging the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while calculating the price.");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingByUserId(Guid userId)
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            if (bookings == null)
            {
                return NotFound("No bookings found.");
            }

            var userBookings = bookings
                .Where(b => b.UserId == userId)
                .Select(b => new BookingDto
                {
                    BookingId = b.BookingId,
                    Status = b.Status,
                    Price = b.Price,
                    UserId = b.UserId.ToString(),
                    Activity = new ActivityDto()
                    {
                        ActivityId = b.ActivityId,
                        ActivityName = b.Activity.ActivityName,
                        Description = b.Activity.Description,
                        Date = b.Activity.Date
                    },
                    //ActivityName = b.Activity.ActivityName  // Assume Activity is eager loaded
                })
                .ToList();

            if (!userBookings.Any())
            {
                return NotFound($"No bookings found for user ID {userId}.");
            }

            return Ok(userBookings);
        }
    }
    public class BookingDto
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public string UserId { get; set; }
        public ActivityDto Activity { get; set; }
    }

    public class ActivityDto
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
