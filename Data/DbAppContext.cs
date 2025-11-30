using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_PROJECT.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_PROJECT.Data
{
    public class DbAppContext : DbContext
    {
        public DbAppContext(DbContextOptions<DbAppContext> options) : base(options)
        {

        }
        
        public DbSet<Expense> Expenses { get; set; }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<RoomEquipment> RoomEquipments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Building> Building { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Building>()
                .ToTable("Buildings");

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Building)
                .WithMany(b => b.Rooms)
                .HasForeignKey(r => r.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Building>().HasData(
                new Building
                {
                    Id = 1,
                    Type = BuildingType.MainBuilding,
                    Address = "ул. Центральная, 1",
                    Description = "Основное здание компании"
                },
                new Building
                {
                    Id = 2,
                    Type = BuildingType.SecondaryBuilding,
                    Address = "ул. Центральная, 1 (корпус Б)",
                    Description = "Второй корпус"
                },
                new Building
                {
                    Id = 3,
                    Type = BuildingType.BusinessCenter,
                    Address = "ул. Деловая, 15",
                    Description = "Дополнительный офис в бизнес-центре"
                }
            );
    
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Category)
                .WithMany(c => c.Rooms)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Type = CategoryType.ConferenceRoom, Description = "Большие залы для совещаний и презентаций" },
                new Category { Id = 2, Type = CategoryType.InterviewRoom, Description = "Комнаты для проведения собеседований" },
                new Category { Id = 3, Type = CategoryType.FocusRoom, Description = "Комнаты для индивидуальной работы" }
            );
    
            modelBuilder.Entity<RoomEquipment>(entity =>
            {
                entity.HasKey(re => new { re.RoomId, re.EquipmentId });
                
                entity.HasOne(re => re.Room)
                      .WithMany(r => r.RoomEquipments)
                      .HasForeignKey(re => re.RoomId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(re => re.Equipment)
                      .WithMany(e => e.RoomEquipments)
                      .HasForeignKey(re => re.EquipmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("Equipments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            modelBuilder.Entity<Equipment>().HasData(
                new Equipment { Id = 1, Name = "Проектор", Description = "Проектор для презентаций" },
                new Equipment { Id = 2, Name = "Маркерная доска", Description = "Белая маркерная доска" },
                new Equipment { Id = 3, Name = "Видеоконференция", Description = "Система для проведения видеоконференций" }
            );

            modelBuilder.Entity<RoomEquipment>().HasData(
                new RoomEquipment { RoomId = 1, EquipmentId = 1, Quantity = 1 },
                new RoomEquipment { RoomId = 1, EquipmentId = 2, Quantity = 1 },
                new RoomEquipment { RoomId = 1, EquipmentId = 3, Quantity = 1 },
                
                new RoomEquipment { RoomId = 2, EquipmentId = 2, Quantity = 1 }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    Name = "Конференц-зал А",
                    Description = "Большой зал для совещаний",
                    Capacity = 20,
                    Location = "3 этаж",
                    CategoryId = 1,
                    BuildingId = 1
                },
                new Room
                {
                    Id = 2,
                    Name = "Переговорная Б",
                    Description = "Небольшая комната для командных встреч",
                    Capacity = 9,
                    Location = "2 этаж",
                    CategoryId = 3,
                    BuildingId = 3
                }
            );

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
                
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = RoleNames.User, Description = "Обычный пользователь" },
                new Role { Id = 2, Name = RoleNames.Admin, Description = "Администратор" }
            );
        }
    }
}