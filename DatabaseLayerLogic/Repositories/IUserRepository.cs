using Microsoft.AspNetCore.Mvc;
using DatabaseLayerLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayerLogic.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetByIdAsync(int id);
        Task InsertAsync(string userName, string passwordHash, string saltValue, DateTime? dateTime);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);

        Task<List<User>> GetByUsername(string username); 
        Task<(string? passwordHash, string? saltValue)> GetUserPasswordHashAndSaltValue(string username);

    }
}
