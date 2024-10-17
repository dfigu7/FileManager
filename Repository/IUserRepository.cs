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
       
        Task<User> AddUser(UserDto user);
       
        Task<User> GetByIdAsync(int userId);
        Task DeleteAsync(int userId);
    }
}
