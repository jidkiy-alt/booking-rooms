using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_PROJECT.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_PROJECT.Data.Service
{
    public class ReviewService : IReviewService
    {
        private readonly DbAppContext _context;

        public ReviewService(DbAppContext context)
        {
            _context = context;
        }

        public async Task AddReviewAsync(Review review)
        {
            review.CreatedAt = DateTime.Now;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Review>> GetRoomReviewsAsync(int roomId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.RoomId == roomId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<double> GetRoomAverageRatingAsync(int roomId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.RoomId == roomId)
                .ToListAsync();

            return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
        }

        public async Task<bool> HasUserReviewedRoomAsync(int userId, int roomId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.UserId == userId && r.RoomId == roomId);
        }
    }
}