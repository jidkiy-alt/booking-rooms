using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_PROJECT.Data.Service;
using ASPNET_PROJECT.Models;
using ASPNET_PROJECT.Models.ViewModels;
using ASPNET_PROJECT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_PROJECT.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService roomService;
        private readonly IUserService userService;
        private readonly IReviewService reviewService;

        public RoomController(IRoomService _roomService, IUserService _userService, IReviewService _reviewService)
        {
            roomService = _roomService;
            userService = _userService;
            reviewService = _reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await roomService.GetAllRoomsAsync();

            var roomRatings = new Dictionary<int, double>();
            foreach (var room in rooms)
            {
                var rating = await reviewService.GetRoomAverageRatingAsync(room.Id);
                roomRatings[room.Id] = rating;
            }

            ViewBag.RoomRatings = roomRatings;
            
            return View(rooms);
        }

        public async Task<IActionResult> Details(int id)
        {
            var room = await roomService.GetRoomByIdAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            var reviews = await reviewService.GetRoomReviewsAsync(id);
            var averageRating = await reviewService.GetRoomAverageRatingAsync(id);
            
            ViewBag.Reviews = reviews;
            ViewBag.AverageRating = averageRating;
            // ViewBag.HasUserReviewed = false;

            var currentUser = await userService.GetCurrentUserAsync();
            if (currentUser != null)
            {
                ViewBag.HasUserReviewed = await reviewService.HasUserReviewedRoomAsync(currentUser.Id, id);
                ViewBag.CurrentUser = currentUser;
            }

            return View(room);
        }

        public async Task<IActionResult> Book(int id)
        {
            var room = await roomService.GetRoomByIdAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            var viewModel = new BookingCreateViewModel
            {
                RoomId = room.Id,
                RoomName = room.Name,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Book(BookingCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (viewModel.EndTime <= viewModel.StartTime)
            {
                ModelState.AddModelError("EndTime", "Время окончания должно быть позже времени начала");
                return View(viewModel);
            }

            if (viewModel.StartTime < DateTime.Now)
            {
                ModelState.AddModelError("StartTime", "Нельзя забронировать комнату в прошлом");
                return View(viewModel);
            }

            try
            {
                var booking = new Booking
                {
                    RoomId = viewModel.RoomId,
                    UserId = userId.Value,
                    Purpose = viewModel.Purpose,
                    StartTime = viewModel.StartTime,
                    EndTime = viewModel.EndTime
                };

                var createdBooking = await roomService.CreateBookingAsync(booking);

                TempData["SuccessMessage"] = $"Комната успешно забронирована на {viewModel.StartTime:dd.MM.yyyy HH:mm} - {viewModel.EndTime:HH:mm}";
                return RedirectToAction("Details", new { id = viewModel.RoomId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(viewModel);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> CreateRoom()
        {
            var user = await userService.GetCurrentUserAsync();
            if (user?.IsAdmin != true)
            {
                TempData["ErrorMessage"] = "Недостаточно прав";
                return RedirectToAction("Index");
            }

            var viewModel = new CreateRoomViewModel
            {
                Categories = await roomService.GetCategoriesAsync(),     
                Buildings = await roomService.GetBuildingsAsync(),        
                Equipments = await roomService.GetEquipmentsAsync()       
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(CreateRoomViewModel viewModel)
        {
            var user = await userService.GetCurrentUserAsync();
            if (user?.IsAdmin != true)
            {
                TempData["ErrorMessage"] = "Недостаточно прав";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {

                return View(viewModel);
            }

            try
            {
                var room = new Room
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Capacity = viewModel.Capacity,
                    Location = viewModel.Location,
                    CategoryId = viewModel.CategoryId,
                    BuildingId = viewModel.BuildingId
                };

                var createdRoom = await roomService.CreateRoomAsync(room);

                if (viewModel.SelectedEquipmentIds.Any())
                {
                    await roomService.AddEquipmentToRoomAsync(createdRoom.Id, viewModel.SelectedEquipmentIds);
                }

                TempData["SuccessMessage"] = $"Комната '{createdRoom.Name}' успешно создана!";
                return RedirectToAction("Details", new { id = createdRoom.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка: {ex.Message}";

                return View(viewModel);
            }
        }
    }
}