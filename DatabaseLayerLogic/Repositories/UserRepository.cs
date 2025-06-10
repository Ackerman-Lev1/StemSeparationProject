using DatabaseLayerLogic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayerLogic.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StemseperationContext _context;
        public UserRepository(StemseperationContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users.FindAsync(id);

        public async Task<IEnumerable<User>> GetUsersAsync() =>
        await _context.Users.ToListAsync();

        public async Task InsertAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetByUsernameAndPassword(string username, string password)
        {
            var user =await _context.Users
                        .Where(u => u.UserName == username && u.Password == password)
                        .ToListAsync();
            return user; 
        }
    }
}
