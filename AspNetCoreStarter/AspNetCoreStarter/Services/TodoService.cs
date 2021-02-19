using AspNetCoreStarter.Dtos;
using AspNetCoreStarter.Mappers;
using AspNetCoreStarter.Models;
using AspNetCoreStarter.Repositories.Interfaces;
using System.Collections.Generic;

namespace AspNetCoreStarter.Services
{
    public class TodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public TodoReadDto Get(int id) => _repository.Get(id).ToReadDto();

        public TodoReadDto GetByUserId(int id) => _repository.GetByUserId(id).ToReadDto();

        public IEnumerable<TodoReadDto> Get() => _repository.Get().ToReadDtos();

        public TodoReadDto Create(int userId, TodoCreateDto dto)
        {
            Todo entity = dto.ToEntity();

            entity.UserId = userId;

            _repository.Create(entity);
            _repository.SaveChanges();

            return entity.ToReadDto();
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
            _repository.SaveChanges();
        }
    }
}
