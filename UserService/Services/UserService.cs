using System;
using System.Threading.Tasks;
using UserService.DTO;
using UserService.Services;
using UserService.Repositories;
using UserService.Entities;
using System.Linq;
using System.Collections.Generic;

namespace UserService.Services
{
    public class UserServiceImpl : IUserService
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public UserServiceImpl(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<GetUser>> GetUsers()
        {            
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new GetUser { Name = u.Name, Email = u.Email, Role = u.Role });
        }

        public async Task<GetUser?> GetUserById(int userId)
        {
            var existing = await _userRepository.GetByIdAsync(userId);
            if (existing == null) return null;

            return new GetUser
            {
                Name = existing.Name,
                Email = existing.Email,
                Role = existing.Role
            };
        }

        public async Task<bool> UpdateUser(int userId, UpdateUser req)
        {
            var existing = await _userRepository.GetByIdAsync(userId);
            if (existing == null) return false;

            existing.Name = string.IsNullOrWhiteSpace(req.Name) ? existing.Name : req.Name;
            existing.Role = string.IsNullOrWhiteSpace(req.Role) ? existing.Role : req.Role;
            existing.Email = string.IsNullOrWhiteSpace(req.Email) ? existing.Email : req.Email;

            await _userRepository.UpdateAsync(existing);
            return true;
        }
    }
}
