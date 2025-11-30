using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASPNET_PROJECT.Models;

namespace ASPNET_PROJECT.ViewModels
{
    public class CreateRoomViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Не должно превышать 100 символов")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(500, ErrorMessage = "Не должно превышать 500 символов")]
        public string Description { get; set; } = null!;

        [Required]
        [Range(1, 100, ErrorMessage = "Значение должно быть от 1 до 100 человек")]
        public int Capacity { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Не должно превышать 100 символов")]
        public string Location { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int BuildingId { get; set; }

        public List<Category> Categories { get; set; } = new();
        public List<Building> Buildings { get; set; } = new();
        public List<Equipment> Equipments { get; set; } = new();

        public List<int> SelectedEquipmentIds { get; set; } = new();
    }
}