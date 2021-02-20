using AspNetCoreStarter.Models;
using System.Collections.Generic;

namespace AspNetCoreStarter.Repositories.Interfaces
{
    public interface ITodoRepository
    {
        Todo Get(int id);
        IEnumerable<Todo> Get();
        IEnumerable<Todo> GetByUserId(int id);
        void Create(Todo Todo);
        void Update(Todo Todo);
        void Delete(int id);
        void SaveChanges();
    }
}
