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
    }
}
