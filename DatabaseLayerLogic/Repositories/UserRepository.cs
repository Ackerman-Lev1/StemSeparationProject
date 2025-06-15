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

        public async Task InsertAsync(string userName, string passwordHash, string saltValue, DateTime? dateTime)
        {
            var user = new User
            {
                UserName = userName,
                PasswordHash = passwordHash,
                SaltValue = saltValue,
                UserCreatedOn = dateTime
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetByUsername(string username)
        {
            var user =await _context.Users
                        .Where(u => u.UserName == username)
                        .ToListAsync();
            return user; 
        }

        public async Task<(string passwordHash, string saltValue)> GetUserPasswordHashAndSaltValue(string userName)
        {
            var result = await _context.Users
                .Where (u => u.UserName == userName)
                .Select(u => new { u.PasswordHash, u.SaltValue })
                .FirstOrDefaultAsync();

            return (result?.PasswordHash, result?.SaltValue);
        }
    }
}
