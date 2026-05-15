using System.Threading.Tasks;
using UserService.Entities;
using UserService.Models;

namespace UserService.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? Error)> RegisterAsync(LoginModal req);
        Task<(bool Success, string? Token, string? Error)> LoginAsync(string username, string password);
    }
}
