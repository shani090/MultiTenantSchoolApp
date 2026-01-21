using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartEduHub.Data;
using SmartEduHub.DTO;
using SmartEduHub.DTO.RegisterResponseDTO;
using SmartEduHub.Interface;
using SmartEduHub.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartEduHub.Repository
{
    public class AuthRepository: IUser
    {
        private readonly SchoolDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AuthRepository(SchoolDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }


        public async Task<LoginResponseDto> LoginAsync(UserLoginDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u =>
                        u.Phone == dto.username || u.Email == dto.username);

                if (user == null)
                {
                    return new LoginResponseDto
                    {
                        IsSuccess = false,
                        Message = dto.username.Contains("@")
                            ? "Invalid email. Please check and try again."
                            : "Invalid phone number. Please check and try again."
                    };
                }

                if (!BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash))
                {
                    return new LoginResponseDto
                    {
                        IsSuccess = false,
                        Message = "Invalid password. Please check and try again."
                    };
                }

                var token = await GenerateJwtToken(user);


                await transaction.CommitAsync();

                return new LoginResponseDto
                {
                    IsSuccess = true,
                    Message = "Login successful",
                    Token = token
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();


                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "Something went wrong. Please try again later."
                };
            }
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            // User specific permissions
            var userClaims = await _context.UserClaims
                .Where(x => x.UserId == user.Id)
                .ToListAsync();

            // College info
            var college = await _context.Colleges
                .Where(x => x.Id == user.CollegeId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim("UserId", user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("CollegeId", user.CollegeId.ToString()),
        new Claim("CollegeName", college?.Name ?? ""),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var groupedClaims = userClaims
                .GroupBy(c => c.ClaimType)
                .Select(g => new Claim(
                    g.Key,
                    string.Join(",", g.Select(x => x.ClaimValue))
                ));

            claims.AddRange(groupedClaims);

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToInt32(_configuration["Jwt:DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

        public async Task<SecureEndpointResponseDto> GetSecureDataAsync(ClaimsPrincipal user)
        {
            try
            {
                var username = user.Identity?.Name;
                var role = user.FindFirst(ClaimTypes.Role)?.Value ?? "";
                var userId = user.FindFirst("UserId")?.Value ?? "";
                var collegeId = user.FindFirst("CollegeId")?.Value ?? "";
                var collegeName = user.FindFirst("CollegeName")?.Value ?? "";

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(collegeId))
                    throw new UnauthorizedAccessException("Invalid token");
                if (!Guid.TryParse(userId, out Guid parsedUserId))
                    throw new UnauthorizedAccessException("Invalid UserId in token");

                int parsedCollegeId = int.Parse(collegeId);

                var collegeInfo = await _context.Colleges
                    .Where(c => c.Id == parsedCollegeId)
                    .Select(c => new { c.Id, c.Name, c.Address })
                    .FirstOrDefaultAsync();

                var userInfo = await _context.Users
                    .Where(u => u.Id == parsedUserId)
                    .Select(u => new { u.Username, u.Email, u.Role })
                    .FirstOrDefaultAsync();

                return new SecureEndpointResponseDto
                {
                    Message = "This is a secure endpoint",
                    Username = username ?? "",
                    Role = role,
                    UserId = userId,
                    CollegeId = collegeId,
                    CollegeName = collegeName,
                    CollegeInfo = collegeInfo,
                    UserInfo = userInfo
                };
            }
            catch ( Exception ex)
            {

                throw ex;
            }
        }
    }
}
