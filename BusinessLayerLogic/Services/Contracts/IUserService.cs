using DatabaseLayerLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerLogic.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task CreateUserAsync(string userName, string passwordHash,string saltValue, DateTime? dateTime);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<List<User>> GetUserByUsername(string username);
        Task<(string? passwordHash, string? saltValue)> GetUserPasswordHashAndSaltValue(string username);
    }
}
