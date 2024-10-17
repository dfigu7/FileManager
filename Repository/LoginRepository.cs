using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DataAccess;
using DataAccess.DTO;
using DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository;

public class LoginRepository : ILoginRepository
{
    private readonly FileManagerDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public LoginRepository(FileManagerDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<string> Login(LoginDto request)
    {
        var user = _context.Users.FirstOrDefault(x => x.Username == request.Username);
        if (user == null) throw new UnauthorizedAccessException("Invalid username or password.");

        using (var hmac = new HMACSHA512(user.PasswordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
            if (!computedHash.SequenceEqual(user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }
        }

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        String id = user.Id.ToString();
        var claims = new List<Claim>
        {
            new Claim("userId", id),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
