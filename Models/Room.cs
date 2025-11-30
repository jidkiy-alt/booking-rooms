using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_PROJECT.Models
{
    public class Room
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = null!;
        
        [Required]
        public string Description { get; set; } = null!;
        
        [Range(1, 50)]
        public int Capacity { get; set; }

        [Required]
        public string Location { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int BuildingId { get; set; }
        public Building Building { get; set; } = null!;
        
        public List<RoomEquipment> RoomEquipments { get; set; } = new();
        
        public List<Booking> Bookings { get; set; } = new();
    }
}