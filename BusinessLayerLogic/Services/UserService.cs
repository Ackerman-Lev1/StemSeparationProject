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

        public async Task CreateUserAsync(User user) =>
            await _userRepository.InsertAsync(user);

        public async Task UpdateUserAsync(User user) =>
            await _userRepository.UpdateAsync(user);

        public async Task DeleteUserAsync(int id) =>
            await _userRepository.DeleteAsync(id);
    }
}
