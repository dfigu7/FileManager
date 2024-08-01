using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly FileManagerDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(FileManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public Task<User> GetById(int id)
        {
            // Implement the method to retrieve a user by id from the data source
            throw new NotImplementedException();
        }

        public Task<User> AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> AddUser(UserDto request)
        {
            var user = _mapper.Map<User>(request);
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public Task<User> UpdateUser(User user)
        {
            // Implement the method to update an existing user in the data source
            throw new NotImplementedException();
        }

        public Task<User> DeleteUser(int id)
        {
            // Implement the method to delete a user by id from the data source
            throw new NotImplementedException();
        }
    }
}