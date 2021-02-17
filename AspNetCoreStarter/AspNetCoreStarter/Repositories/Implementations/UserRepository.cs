using AspNetCoreStarter.Context;
using AspNetCoreStarter.Models;
using AspNetCoreStarter.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreStarter.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AspNetCoreStarterContext _context;

        public UserRepository(AspNetCoreStarterContext context)
        {
            _context = context;
        }

        public User Get(int id) => _context.Users.Find(id);

        public User Get(string email) => 
            _context.Users.FirstOrDefault(u => u.Email == email);

        public IEnumerable<User> Get() => _context.Users;

        public void Create(User user) => _context.Users.Add(user);

        public void Update(User user) { }

        public void Delete(int id)
        {
            User user = _context.Users.Find(id);

            if (user == null) return;

            _context.Users.Remove(user);
        }

        public void SaveChanges() => _context.SaveChanges();
    }
}
