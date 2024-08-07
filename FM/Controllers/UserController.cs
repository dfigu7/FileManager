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
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFileItemService _fileItemService;
        private readonly StorageSettings _storageSettings;
        private readonly IFolderRepository _folderRepository;
        private readonly IFolderService _folderService;
        private readonly IUserRepository _userRepository;
        private readonly UserIdProviderService _userIdProviderService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork,IFileItemService fileItemService, IOptions<StorageSettings> storageSettings,
            IFolderRepository folderRepository, IFolderService folderService, IUserService userService, IUserRepository userRepository)
        {
            _fileItemService = fileItemService;
            _storageSettings = storageSettings.Value;
            _folderRepository = folderRepository;
            _folderService = folderService;
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
    }
}
