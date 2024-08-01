using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTO;
using DataAccess.Entities;

namespace DataAccess.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> AddUser(UserDto user);
        Task<User> UpdateUser(UserDto user);
        Task<User> DeleteUser(int id);
    }
}
