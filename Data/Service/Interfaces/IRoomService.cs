using ASPNET_PROJECT.Models;
using ASPNET_PROJECT.ViewModels;

namespace ASPNET_PROJECT.Data.Service
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room?> GetRoomByIdAsync(int id);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime start, DateTime end);

        Task<Booking> CreateBookingAsync(Booking booking);
        Task<bool> CancelBookingAsync(int bookingId);

        Task<EditRoomEquipmentViewModel> GetRoomEquipmentForEditAsync(int roomId);
        Task UpdateRoomEquipmentAsync(int roomId, List<int> selectedEquipmentIds);

        Task<Room> CreateRoomAsync(Room room);
        Task AddEquipmentToRoomAsync(int roomId, List<int> equipmentIds);


        Task<List<Category>> GetCategoriesAsync();
        Task<List<Building>> GetBuildingsAsync();
        Task<List<Equipment>> GetEquipmentsAsync();

        Task<List<Booking>> GetAllBookingsWithDetailsAsync();
        
        Task<bool> DeleteRoomAsync(int roomId);
    }
}