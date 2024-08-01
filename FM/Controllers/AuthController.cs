using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DataAccess;
using DataAccess.DTO;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FileManagerDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(FileManagerDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Handle user Registration
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                {
                    return BadRequest(new { message = "Username is taken" });
                }

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User newUser = new User
                {
                    Username = request.Username,
                    Name = request.Name,
                    LastName = request.LastName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (user != null)
                {
                    string token = CreateToken(user);
                    return Ok(new { message = "Registration successful", token = token });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while registering the user.", exception = ex.Message });
            }

            return null;
        }

        // Handle User Login 
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return BadRequest(new { message = "Wrong Password" });
                }

                string token = CreateToken(user);
                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while logging in.", exception = ex.Message });
            }
        }

        // Handle password hashing
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // Handle password verification
        public static bool VerifyPasswordHash(string inputedPassword, byte[] userPasswordHash, byte[] userPasswordSalt)
        {
            using (var hmac = new HMACSHA512(userPasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputedPassword));
                return computedHash.SequenceEqual(userPasswordHash);
            }
        }

        // Handle Token 
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var value = _configuration.GetSection("Jwt:Key").Value;
            if (value != null)
            {
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }

            return null;
        }

    }
}