using Microsoft.AspNetCore.Mvc;
using ASPNET_PROJECT.Models;
using Microsoft.EntityFrameworkCore;
using ASPNET_PROJECT.Data;

namespace ASPNET_PROJECT.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersApiController : ControllerBase
    {
        private readonly DbAppContext _context;

        public UsersApiController(DbAppContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    Role = u.Role.Name,
                    u.CreatedAt,
                    IsAdmin = u.IsAdmin
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Bookings)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { message = "Пользователь не найден" });
            }

            var result = new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                Role = new { user.Role.Id, user.Role.Name },
                user.CreatedAt,
                IsAdmin = user.IsAdmin,
                Bookings = user.Bookings.Select(b => new
                {
                    b.Id,
                    b.Purpose,
                    StartTime = b.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    EndTime = b.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Status = b.Status.ToString()
                })
            };

            return Ok(result);
        }
    }
}