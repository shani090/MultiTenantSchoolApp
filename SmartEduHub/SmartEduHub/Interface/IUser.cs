using SmartEduHub.DTO;
using SmartEduHub.Models;

namespace SmartEduHub.Interface
{
    public interface IUser
    {
        Task<bool> RegisterAsync(RegisterDTO dto);
        Task<User?> LoginAsync(LoginDTO dto);
    }
}
