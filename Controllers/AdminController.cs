using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_PROJECT.Data.Service;
using ASPNET_PROJECT.Models;
using ASPNET_PROJECT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_PROJECT.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRoomService roomService;
        private readonly IUserService userService;

        public AdminController(IRoomService _roomService, IUserService _userService)
        {
            roomService = _roomService;
            userService = _userService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userService.GetCurrentUserAsync();
            if (user?.IsAdmin != true)
            {
                TempData["ErrorMessage"] = "Недостаточно прав";
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        public async Task<IActionResult> AllBookings()
        {
            var user = await userService.GetCurrentUserAsync();
            if (user?.IsAdmin != true)
            {
                TempData["ErrorMessage"] = "Недостаточно прав";
                return RedirectToAction("Login", "Auth");
            }

            var bookings = await roomService.GetAllBookingsWithDetailsAsync();
            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> CreateRoom()
        {
            var user = await userService.GetCurrentUserAsync();
            if (user?.IsAdmin != true)
            {
                TempData["ErrorMessage"] = "Недостаточно прав";
                return RedirectToAction("Login", "Auth");
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
                return RedirectToAction("Login", "Auth");
            }

            if (viewModel.Capacity < 1 || viewModel.Capacity > 50)
            {
                ModelState.AddModelError("Capacity", "Вместимость должна быть от 1 до 50 человек");
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
                return RedirectToAction("Details", "Room", new { id = createdRoom.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка: {ex.Message}";

                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var user = await userService.GetCurrentUserAsync();
            if (user?.IsAdmin != true)
            {
                TempData["ErrorMessage"] = "Недостаточно прав";
                return RedirectToAction("Login", "Auth");
            }

            var room = await roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                TempData["ErrorMessage"] = "Комната не найдена";
                return RedirectToAction("Index", "Room");
            }

            return View(room); 
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoomConfirmed(int id)
        {
            var user = await userService.GetCurrentUserAsync();
            if (user?.IsAdmin != true)
            {
                TempData["ErrorMessage"] = "Недостаточно прав";
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var result = await roomService.DeleteRoomAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Комната успешно удалена";
                }
                else
                {
                    TempData["ErrorMessage"] = "Комната не найдена";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка при удалении комнаты: {ex.Message}";
            }

            return RedirectToAction("Index", "Room");
        }
    }
}