using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEduHub.DTO;
using SmartEduHub.Interface;

namespace SmartEduHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUser _user;
        private readonly ITenantContextAccessor _tenantContextAccessor;
        private readonly ILogger<AuthController> _logger;
        private readonly int _tenantId;

        public AuthController(
            IUser user,
            ITenantContextAccessor tenantContextAccessor,
            ILogger<AuthController> logger)
        {
            _user = user;
            _tenantContextAccessor = tenantContextAccessor;
            _logger = logger;

            // TenantId header se nikal rahe hain
            int.TryParse(_tenantContextAccessor.GetTenantId(), out _tenantId);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {

                var result = await _user.Register(registerDto, _tenantId);

                if (result.Message == "PhoneNumber already exists")
                {
                    return Conflict(new { status = "error", message = result.Message });
                }
                if (result.Message == "Email already exists")
                {
                    return StatusCode(500, new { status = "error", message = result.Message });
                }
                if (result.Message == "Password must be at least 6 characters long")
                {
                    return BadRequest(new { status = "error", message = result.Message });
                }
                if (result.Message == "Password must contain at least one special character")
                {
                    return BadRequest(new { status = "error", message = result.Message });
                }

                return Ok(new
                {
                    status = "success",
                    id = result.Id,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering a user.");
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.username) ||
                string.IsNullOrWhiteSpace(loginDto.PasswordHash))
                {
                    return BadRequest(new { status = "error", message = "Email/Phone and password are required" });
                }
                var authResult = await _user.LoginAsync(loginDto);


                return Ok(new
                {
                    authResult
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering a user.");
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        [HttpGet("secure")]
        [Authorize]
        public async Task<IActionResult> SecureEndpoint()
        {
            try
            {
                var result = await _user.GetSecureDataAsync(User);
                return Ok(result);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                _logger.LogWarning(uaEx, "Unauthorized access in SecureEndpoint");
                return Unauthorized(new { status = "error", message = uaEx.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in SecureEndpoint for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { status = "error", message = "An internal server error occurred" });
            }
        }

    }
}
