using System.Collections.Generic;
using System.Threading.Tasks;
using ASPNET_PROJECT.Models;

namespace ASPNET_PROJECT.Data.Service
{
    public interface IReviewService
    {
        Task AddReviewAsync(Review review);
        Task<List<Review>> GetRoomReviewsAsync(int roomId);
        Task<double> GetRoomAverageRatingAsync(int roomId);
        Task<bool> HasUserReviewedRoomAsync(int userId, int roomId);
    }
}