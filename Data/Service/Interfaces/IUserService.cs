using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_PROJECT.Models;

namespace ASPNET_PROJECT.Data.Service
{
    public interface IUserService
    {
        Task<User?> GetCurrentUserAsync();
    }
}