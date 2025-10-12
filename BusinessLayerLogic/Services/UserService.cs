using BusinessLayerLogic.Services.Contracts;
using DatabaseLayerLogic.Models;
using DatabaseLayerLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() =>
            await _userRepository.GetUsersAsync();

        public async Task<User?> GetUserByIdAsync(int id) =>
            await _userRepository.GetByIdAsync(id);

        public async Task CreateUserAsync(string userName, string passwordHash, string saltValue, DateTime? dateTime) =>
            await _userRepository.InsertAsync(userName,passwordHash,saltValue,dateTime);

        public async Task UpdateUserAsync(User user) =>
            await _userRepository.UpdateAsync(user);

        public async Task DeleteUserAsync(int id) =>
            await _userRepository.DeleteAsync(id);

        public async Task<List<User>> GetUserByUsername(string username) =>
            await _userRepository.GetByUsername(username);
        public async Task<(string? passwordHash, string? saltValue)> GetUserPasswordHashAndSaltValue(string username) =>
            await _userRepository.GetUserPasswordHashAndSaltValue(username);
        public async Task<User> GetOrCreateGoogleUserAsync(string email, string? firstName, string? lastName)
            => await _userRepository.GetOrCreateGoogleUserAsync(email, firstName, lastName);
    }
}
