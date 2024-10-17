using BLL.Services;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _loginService.Login(loginDto);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
