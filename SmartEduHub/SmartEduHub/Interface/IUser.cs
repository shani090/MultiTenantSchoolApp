using SmartEduHub.DTO;
using SmartEduHub.DTO.RegisterResponseDTO;
using SmartEduHub.Models;

namespace SmartEduHub.Interface
{
    public interface IUser
    {
        Task<RegisterResponseDto> Register(RegisterDTO objReg, int _tanentID);
        Task<User?> LoginAsync(LoginDTO dto);
    }
}
