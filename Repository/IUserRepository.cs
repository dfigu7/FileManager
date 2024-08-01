using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTO;


namespace Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> AddUser(UserDto user);
        Task<User> UpdateUser(User user);
        Task<User> DeleteUser(int id);
    }
}
