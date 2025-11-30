using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_PROJECT.Models
{
    public class RoomEquipment
    {
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; } = null!;
        
        public int Quantity { get; set; } = 1;
    }
}