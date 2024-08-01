using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Repository
{
    public interface ILoginRepository
    {
        Task<string> Login(LoginDto requset);
    }
}
