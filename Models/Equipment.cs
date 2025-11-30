using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_PROJECT.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        
        [StringLength(500)]
        public string Description { get; set; } = null!;
        
        public List<RoomEquipment> RoomEquipments { get; set; } = new();
    }
}