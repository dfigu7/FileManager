using DataAccess.DTO;
using Repository;

namespace BLL.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<object> Login(LoginDto request)
        {
            var token = await _loginRepository.Login(request);

            return new { accessToken = token };
        }
    }
}
