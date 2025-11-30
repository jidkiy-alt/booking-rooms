using ASPNET_PROJECT.Models;

namespace ASPNET_PROJECT.Data.Service.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(string email, string password, string firstName, string lastName);
        Task<User?> LoginAsync(string email, string password);
        Task<User?> GetUserByIdAsync(int id);
    }
}