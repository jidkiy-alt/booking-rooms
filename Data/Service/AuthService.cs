using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_PROJECT.Data.Service.Interfaces;
using ASPNET_PROJECT.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_PROJECT.Data.Service
{
    public class AuthService(DbAppContext dbAppContext) : IAuthService
    {
        private readonly DbAppContext dbContext = dbAppContext;

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
                
            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await dbContext.Users.FindAsync(id);
        }

        public async Task<User?> RegisterAsync(string email, string password, string firstName, string lastName)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                
            if (existingUser != null)
                return null;

            var user = new User
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.Now
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            
            return user;
        }
    }
}