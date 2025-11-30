using Microsoft.AspNetCore.Mvc;
using ASPNET_PROJECT.Models;
using ASPNET_PROJECT.Data.Service;
using Microsoft.EntityFrameworkCore;
using ASPNET_PROJECT.Data;


namespace ASPNET_PROJECT.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsApiController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly DbAppContext _context;

        public RoomsApiController(IRoomService roomService, DbAppContext context)
        {
            _roomService = roomService;
            _context = context;
        }

        // GET: api/rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();

            var result = rooms.Select(r => new
            {
                r.Id,
                r.Name,
                r.Description,
                r.Capacity,
                r.Location,
                Category = r.Category.DisplayName,
                Building = r.Building.DisplayName,
                Equipment = r.RoomEquipments.Select(re => new
                {
                    re.Equipment.Name,
                    re.Quantity
                })
            });

            return Ok(result);
        }
        
        // GET: api/rooms/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);

            if (room == null)
            {
                return NotFound(new { message = "Комната не найдена" });
            }
            
            var reviews = await _context.Reviews
                .Where(r => r.RoomId == id)
                .Include(r => r.User)
                .Select(rev => new
                {
                    rev.Id,
                    User = new { rev.User.FirstName, rev.User.LastName },
                    rev.Rating,
                    rev.Comment,
                    rev.CreatedAt
                })
                .ToListAsync();

            var result = new
            {
                room.Id,
                room.Name,
                room.Description,
                room.Capacity,
                room.Location,
                Category = new 
                { 
                    room.Category.Id, 
                    room.Category.DisplayName 
                },
                Building = new 
                { 
                    room.Building.Id, 
                    room.Building.DisplayName,
                    room.Building.Address 
                },
                Equipment = room.RoomEquipments.Select(re => new 
                { 
                    re.Equipment.Id,
                    re.Equipment.Name,
                    re.Equipment.Description,
                    re.Quantity 
                }),
                Reviews = reviews
            };

            return Ok(result);
        }
    }
}