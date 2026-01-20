using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartEduHub.Data;
using SmartEduHub.DTO;
using SmartEduHub.DTO.RegisterResponseDTO;
using SmartEduHub.Interface;
using SmartEduHub.Models;

namespace SmartEduHub.Repository
{
    public class AuthRepository: IUser
    {
        private readonly SchoolDbContext _context;
        private readonly IMapper _mapper;
        public AuthRepository(SchoolDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        

        public Task<User?> LoginAsync(LoginDTO dto)
        {
            throw new NotImplementedException();
        }
        public async Task<RegisterResponseDto> Register(RegisterDTO objReg, int tenantId)  
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();  
            try
            {
                var entity = _mapper.Map<User>(objReg);
                entity.CollegeId = objReg.CollegeId ?? entity.CollegeId;
                // Important: Validate PLAIN password (from DTO), not the future hash!
                if (string.IsNullOrWhiteSpace(objReg.PasswordHash) || objReg.PasswordHash.Length < 6)
                {
                    return new RegisterResponseDto { Message = "Password must be at least 6 characters long" };
                }

                if (!objReg.PasswordHash.Any(ch => !char.IsLetterOrDigit(ch)))
                {
                    return new RegisterResponseDto { Message = "Password must contain at least one special character" };
                }

                if (await _context.Users.AnyAsync(u => u.Phone == objReg.PhoneNumber))  // better use DTO value directly if possible
                {
                    return new RegisterResponseDto { Message = "PhoneNumber already exists" };
                }

                if (await _context.Users.AnyAsync(u => u.Email == objReg.Email))
                {
                    return new RegisterResponseDto { Message = "Email already exists" };
                }

                entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(objReg.PasswordHash);  // ← hash the PLAIN password!
                entity.Role = entity.Role?.ToLowerInvariant();

                _context.Users.Add(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new RegisterResponseDto
                {
                    Id = entity.Id,
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;  
            }
        }

    }
}
