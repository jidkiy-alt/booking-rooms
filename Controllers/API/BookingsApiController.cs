using Microsoft.AspNetCore.Mvc;
using ASPNET_PROJECT.Models;
using ASPNET_PROJECT.Data.Service;
using ASPNET_PROJECT.Data;
using Microsoft.EntityFrameworkCore;


namespace ASPNET_PROJECT.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsApiController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly DbAppContext _context;

        public BookingsApiController(IRoomService roomService, DbAppContext context)
        {
            _roomService = roomService;
            _context = context;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();

            var result = bookings.Select(b => new
            {
                b.Id,
                Room = new { b.Room.Id, b.Room.Name },
                User = new { b.User.Id, b.User.FirstName, b.User.LastName },
                b.Purpose,
                StartTime = b.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                EndTime = b.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                Status = b.Status.ToString(),
                b.CreatedAt
            });

            return Ok(result);
        }

        // POST: api/bookings (2026-01-01T10:00:00)
        [HttpPost]
        public async Task<ActionResult<object>> CreateBooking([FromBody] BookingCreateApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            if (model.EndTime <= model.StartTime)
            {
                return BadRequest(new { message = "Время окончания должно быть позже времени начала" });
            }

            if (model.StartTime < DateTime.Now)
            {
                return BadRequest(new { message = "Время старта позже времени начала бронирования" });
            }

            try
            {
                var booking = new Booking
                {
                    RoomId = model.RoomId,
                    UserId = model.UserId,
                    Purpose = model.Purpose,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    Status = BookingStatus.Confirmed
                };

                var createdBooking = await _roomService.CreateBookingAsync(booking);

                var result = new
                {
                    createdBooking.Id,
                    message = "Бронирование успешно создано",
                    Room = new { createdBooking.Room.Id, createdBooking.Room.Name },
                    StartTime = createdBooking.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    EndTime = createdBooking.EndTime.ToString("yyyy-MM-ddTHH:mm:ss")
                };

                return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/bookings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound(new { message = "Бронирование не найдено" });
            }

            var result = new
            {
                booking.Id,
                Room = new { booking.Room.Id, booking.Room.Name },
                User = new { booking.User.Id, booking.User.FirstName, booking.User.LastName, booking.User.Email },
                booking.Purpose,
                StartTime = booking.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                EndTime = booking.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                Status = booking.Status.ToString(),
                booking.CreatedAt
            };

            return Ok(result);
        }
    }

    public class BookingCreateApiModel
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string Purpose { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}