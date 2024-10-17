using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTO;

namespace BLL.Services
{
    public interface ILoginService
    {
        Task<object> Login(LoginDto request);
    }
}
