using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_PROJECT.Models
{
    public class Review
    {
        public int Id { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string Comment { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
    }
}