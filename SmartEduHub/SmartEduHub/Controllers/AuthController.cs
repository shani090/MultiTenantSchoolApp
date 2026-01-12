using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartEduHub.Interface;

namespace SmartEduHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUser _user;
        public AuthController(IUser user)
        {
            _user = user;
        }
    }
}
