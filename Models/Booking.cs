using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_PROJECT.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        [Required]
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
        
        [Required]
        public string Purpose { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    }
    
    public enum BookingStatus
    {
        Confirmed,
        Cancelled,
        Completed
    }
}