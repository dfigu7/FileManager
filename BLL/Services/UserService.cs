using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.DTO;
using DataAccess.Entities;
using DataAccess.Services;
using Repository;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private IUserService _userServiceImplementation;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            
        }


        public Task<IEnumerable<User>> GetAll()
        {
            var users = _userRepository.GetAll();
            return users;
        }

        public Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> AddUser(UserDto user)
        {
            var newUser = await _userRepository.AddUser(user);
            return newUser;
        }

        public Task<User> UpdateUser(UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<User> DeleteUser(int id)
        {
            return _userServiceImplementation.DeleteUser(id);
        }

        public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        await _userRepository.DeleteAsync(userId);
        return true;
    }



    }
}
