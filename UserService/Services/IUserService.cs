using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.DTO;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<IEnumerable<GetUser>> GetUsers();
        Task<GetUser?> GetUserById(int userId);
        Task<bool> UpdateUser(int userId, UpdateUser req);
    }
}
