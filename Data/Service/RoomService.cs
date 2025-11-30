using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_PROJECT.Models;
using ASPNET_PROJECT.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_PROJECT.Data.Service
{
    public class RoomService : IRoomService
    {
        private readonly DbAppContext _context;
        private readonly INotificationService _notificationService;

        public RoomService(DbAppContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms
                .Include(r => r.RoomEquipments)
                    .ThenInclude(re => re.Equipment)
                .Include(r => r.Category)
                .Include(r => r.Building)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            // подгружаем связанные строки из Bookings, чтобы можно было к ним обратиться через room
            return await _context.Rooms
                .Include(r => r.Bookings)
                    .ThenInclude(b => b.User)
                .Include(r => r.RoomEquipments)
                    .ThenInclude(re => re.Equipment)
                .Include(r => r.Category)
                .Include(r => r.Building)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime start, DateTime end)
        {
            var conflictingBookings = await _context.Bookings
                .Where(b => b.RoomId == roomId &&
                           b.Status == BookingStatus.Confirmed &&
                           b.StartTime < end &&
                           b.EndTime > start)
                .AnyAsync();

            return !conflictingBookings;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            var isAvailable = await IsRoomAvailableAsync(booking.RoomId, booking.StartTime, booking.EndTime);

            if (!isAvailable)
            {
                throw new InvalidOperationException("Комната уже забронирована на выбранное время");
            }

            booking.Status = BookingStatus.Confirmed;
            booking.CreatedAt = DateTime.Now;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            await _notificationService.CreateNotificationAsync(
                title: "Бронирование создано",
                message: $"Вы забронировали комнату на {booking.StartTime:dd.MM.yyyy HH:mm}",
                type: NotificationType.BookingCreated,
                userId: booking.UserId,
                bookingId: booking.Id
            );

            var createdBooking = await _context.Bookings
                .Include(b => b.User)    // загружаем пользователя
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == booking.Id);

            return createdBooking!;
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null)
                return false;

            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();

            return true;
        }

        // берём список всего оборудования, после список оборо=удования в комнате, и прохоимся по этим спискам в поисках пересечений айдишников
        public async Task<EditRoomEquipmentViewModel> GetRoomEquipmentForEditAsync(int roomId)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomEquipments)
                .ThenInclude(re => re.Equipment)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null)
                throw new ArgumentException("Комната не найдена");

            var allEquipments = await _context.Equipments.ToListAsync();

            var viewModel = new EditRoomEquipmentViewModel
            {
                RoomId = room.Id,
                RoomName = room.Name,
                RoomDescription = room.Description,
                AvailableEquipments = allEquipments.Select(e => new EquipmentItem
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    IsSelected = room.RoomEquipments.Any(re => re.EquipmentId == e.Id)
                }).ToList()
            };

            return viewModel;
        }

        public async Task UpdateRoomEquipmentAsync(int roomId, List<int> selectedEquipmentIds)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomEquipments)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null)
                throw new ArgumentException("Комната не найдена");

            // удаляем старые связи
            _context.RoomEquipments.RemoveRange(room.RoomEquipments);

            foreach (var equipmentId in selectedEquipmentIds)
            {
                var roomEquipment = new RoomEquipment
                {
                    RoomId = roomId,
                    EquipmentId = equipmentId,
                    Quantity = 1
                };

                _context.RoomEquipments.Add(roomEquipment);
            }

            await _context.SaveChangesAsync();
        }
        
        public async Task<Room> CreateRoomAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return room;
        }

        public async Task AddEquipmentToRoomAsync(int roomId, List<int> equipmentIds)
        {
            foreach (var equipmentId in equipmentIds)
            {
                var roomEquipment = new RoomEquipment
                {
                    RoomId = roomId,
                    EquipmentId = equipmentId,
                    Quantity = 1
                };
                _context.RoomEquipments.Add(roomEquipment);
            }
            await _context.SaveChangesAsync();
        }


        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<Building>> GetBuildingsAsync()
        {
            return await _context.Building.ToListAsync();
        }

        public async Task<List<Equipment>> GetEquipmentsAsync()
        {
            return await _context.Equipments.ToListAsync();
        }

        public async Task<List<Booking>> GetAllBookingsWithDetailsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Building)
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .Include(b => b.User)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<bool> DeleteRoomAsync(int roomId)
        {
            var room = await _context.Rooms
                .Include(r => r.Bookings)
                .Include(r => r.RoomEquipments)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null)
                return false;

            var bookingIds = room.Bookings.Select(b => b.Id).ToList();

            if (bookingIds.Any())
            {
                await _context.Notifications
                    .Where(n => bookingIds.Contains(n.BookingId.Value))
                    .ExecuteDeleteAsync();
            }

            _context.Bookings.RemoveRange(room.Bookings);
            _context.RoomEquipments.RemoveRange(room.RoomEquipments);
            
            _context.Rooms.Remove(room);
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}