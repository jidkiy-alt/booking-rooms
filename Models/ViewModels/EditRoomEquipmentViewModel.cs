using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_PROJECT.ViewModels
{
    public class EditRoomEquipmentViewModel
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public string RoomDescription { get; set; } = null!;
        
        public List<EquipmentItem> AvailableEquipments { get; set; } = new();
    }

    public class EquipmentItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}