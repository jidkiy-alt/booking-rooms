using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_PROJECT.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;      
        public string Message { get; set; } = null!;     
        public DateTime CreatedAt { get; set; }        
        public bool IsRead { get; set; }                 
        public NotificationType Type { get; set; }     
        
        public int UserId { get; set; }             
        public User User { get; set; } = null!;
        
        public int? BookingId { get; set; } 
        public Booking? Booking { get; set; }
    }

    public enum NotificationType
    {
        BookingCreated = 1,      
        BookingReminder = 2,     
        BookingCancelled = 3,    
        SystemAlert = 4          
    }
}