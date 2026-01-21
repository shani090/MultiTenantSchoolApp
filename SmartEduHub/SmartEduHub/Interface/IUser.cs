using SmartEduHub.DTO;
using SmartEduHub.DTO.RegisterResponseDTO;
using SmartEduHub.Models;
using System.Security.Claims;

namespace SmartEduHub.Interface
{
    public interface IUser
    {
        Task<RegisterResponseDto> Register(RegisterDTO objReg, int _tanentID);
        Task<LoginResponseDto> LoginAsync(UserLoginDto dto);
        Task<SecureEndpointResponseDto> GetSecureDataAsync(ClaimsPrincipal user);
    }
}
