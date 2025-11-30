using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_PROJECT.Models;

namespace ASPNET_PROJECT.Data.Service
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string title, string message, NotificationType type, int userId, int? bookingId = null);
        Task<List<Notification>> GetUserNotificationsAsync(int userId);
        Task<List<Notification>> GetUnreadNotificationsAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }
}