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
        Task InsertAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);

        Task<List<User>> GetByUsernameAndPassword(string username, string password); 

    }
}
