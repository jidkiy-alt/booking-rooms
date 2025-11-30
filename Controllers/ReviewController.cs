using Microsoft.AspNetCore.Mvc;
using ASPNET_PROJECT.Data.Service;
using ASPNET_PROJECT.Models;

namespace ASPNET_PROJECT.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;

        public ReviewController(IReviewService reviewService, IUserService userService)
        {
            _reviewService = reviewService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int roomId, int rating, string comment)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Auth");

            var hasReviewed = await _reviewService.HasUserReviewedRoomAsync(user.Id, roomId);
            if (hasReviewed)
            {
                TempData["ErrorMessage"] = "Вы уже оставляли отзыв для этой комнаты";
                return RedirectToAction("Details", "Room", new { id = roomId });
            }

            var review = new Review
            {
                Rating = rating,
                Comment = comment,
                UserId = user.Id,
                RoomId = roomId
            };

            await _reviewService.AddReviewAsync(review);
            TempData["SuccessMessage"] = "Отзыв добавлен!";

            return RedirectToAction("Details", "Room", new { id = roomId });
        }
    }
}