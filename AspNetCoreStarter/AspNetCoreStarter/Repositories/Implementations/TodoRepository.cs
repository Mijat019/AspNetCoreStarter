using AspNetCoreStarter.Context;
using AspNetCoreStarter.Models;
using AspNetCoreStarter.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreStarter.Repositories.Implementations
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AspNetCoreStarterContext _context;

        public TodoRepository(AspNetCoreStarterContext context)
        {
            _context = context;
        }

        public Todo Get(int id) => _context.Todos.Find(id);

        public Todo GetByUserId(int id) => _context.Todos.FirstOrDefault(u => u.UserId == id);

        public IEnumerable<Todo> Get() => _context.Todos;

        public void Create(Todo Todo) => _context.Todos.Add(Todo);

        public void Update(Todo Todo) { }

        public void Delete(int id)
        {
            Todo todo = _context.Todos.Find(id);

            if (todo == null) return;

            _context.Todos.Remove(todo);
        }

        public void SaveChanges() => _context.SaveChanges();

    }
}
