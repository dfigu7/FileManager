using BLL.Services;
using BLL;
using DataAccess.DTO;
using DataAccess.Services;
using FileManager.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repository;
using DataAccess.Entities;

namespace FMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

       
    
        private readonly IUserRepository _userRepository;
       
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork,   IUserService userService, IUserRepository userRepository)
        {
    
           
            _userService = userService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _unitOfWork.Users.GetAll();
            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto user)
        {
            var newUser = await _userService.AddUser(user);
            return Ok(newUser);
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
            {
                return NotFound(new { message = "User not found" });
            }

            return NoContent();
        }
    }
}
