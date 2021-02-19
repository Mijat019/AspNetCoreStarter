using AspNetCoreStarter.Models;
using System.Collections.Generic;

namespace AspNetCoreStarter.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User Get(int id);
        User Get(string email);
        IEnumerable<User> Get();
        void Create(User user);
        void Update(User user);
        void Delete(int id);
        void SaveChanges();
    }
}
