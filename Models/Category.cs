using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_PROJECT.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public CategoryType Type { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = null!;

        public List<Room> Rooms { get; set; } = new();

        public string DisplayName => Type switch
        {
            CategoryType.ConferenceRoom => "Конференц-зал",
            CategoryType.InterviewRoom => "Комната для собеседований",
            CategoryType.FocusRoom => "Фокус-комната",
            _ => Type.ToString()
        };
    }

    public enum CategoryType
    {
        ConferenceRoom = 1,
        InterviewRoom = 2,
        FocusRoom = 3,
    }
}