using ASPNET_PROJECT.Data.Service;
using ASPNET_PROJECT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_PROJECT.Controllers
{
    public class RoomEquipmentController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomEquipmentController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var viewModel = await _roomService.GetRoomEquipmentForEditAsync(id);
                return View(viewModel);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoomEquipmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
                
                var freshData = await _roomService.GetRoomEquipmentForEditAsync(viewModel.RoomId);
                viewModel.AvailableEquipments = freshData.AvailableEquipments;
                viewModel.RoomName = freshData.RoomName;
                viewModel.RoomDescription = freshData.RoomDescription;
                return View(viewModel);
            }

            try
            {
                var selectedIds = new List<int>();
                if (Request.Form["SelectedEquipmentIds"].Count > 0)
                {
                    selectedIds = Request.Form["SelectedEquipmentIds"]
                        .Where(id => !string.IsNullOrEmpty(id))
                        .Select(id => int.Parse(id))
                        .ToList();
                }

                await _roomService.UpdateRoomEquipmentAsync(viewModel.RoomId, selectedIds);

                TempData["SuccessMessage"] = "Оборудование комнаты успешно обновлено";
                return RedirectToAction("Details", "Room", new { id = viewModel.RoomId });
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}