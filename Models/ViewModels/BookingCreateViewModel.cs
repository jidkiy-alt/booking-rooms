using System.ComponentModel.DataAnnotations;

namespace ASPNET_PROJECT.Models.ViewModels
{
    public class BookingCreateViewModel
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;

        [Required]
        [Display(Name = "Цель бронирования")]
        public string Purpose { get; set; } = null!;

        [Required]
        [Display(Name = "Начало бронирования")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; } = DateTime.Now.AddHours(1);

        [Required]
        [Display(Name = "Окончание бронирования")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; } = DateTime.Now.AddHours(2);

        public bool IsValidPeriod => EndTime > StartTime;
    }
}