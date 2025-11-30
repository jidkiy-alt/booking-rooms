using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_PROJECT.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int RoleId { get; set; } = 1;
        public Role Role { get; set; } = null!;

        public List<Booking> Bookings { get; set; } = new();
        public bool IsAdmin => Role?.Name == "Admin";
    }
    
    public enum UserRole
    {
        User,       
        Admin
    }
}