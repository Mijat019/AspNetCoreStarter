using AspNetCoreStarter.Dtos;
using AspNetCoreStarter.Exceptions;
using AspNetCoreStarter.Helpers;
using AspNetCoreStarter.Mappers;
using AspNetCoreStarter.Models;
using AspNetCoreStarter.Repositories.Interfaces;
using System.Collections.Generic;

namespace AspNetCoreStarter.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public UserReadDto Get(int id) => _repository.Get(id).ToReadDto();

        public User Get(string email) => _repository.Get(email);

        public IEnumerable<UserReadDto> Get() => _repository.Get().ToReadDtos();

        public UserReadDto Create(UserCreateDto dto)
        {
            User entity = _repository.Get(dto.Email);

            if (entity != null) throw new BusinessException("User with that email already exists.", 400);

            entity = dto.ToEntity();

            entity.Password = HashHelper.Hash(entity.Password);

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
