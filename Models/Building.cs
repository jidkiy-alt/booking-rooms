using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_PROJECT.Models
{
    public class Building
    {
        public int Id { get; set; }
        
        [Required]
        public BuildingType Type { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Address { get; set; } = null!;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public List<Room> Rooms { get; set; } = new();
        
        public string DisplayName => Type switch
        {
            BuildingType.MainBuilding => "Главный корпус",
            BuildingType.SecondaryBuilding => "Корпус Б",
            BuildingType.BusinessCenter => "Бизнес-центр",
            _ => Type.ToString()
        };
    }
    
    public enum BuildingType
    {
        MainBuilding = 1,
        SecondaryBuilding = 2,
        BusinessCenter = 3,
    }
}