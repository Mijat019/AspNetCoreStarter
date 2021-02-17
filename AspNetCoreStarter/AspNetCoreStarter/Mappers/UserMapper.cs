using AspNetCoreStarter.Dtos;
using AspNetCoreStarter.Models;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreStarter.Mappers
{
    public static class UserMapper
    {
        public static UserReadDto ToReadDto(this User entity) => new UserReadDto
        {
            Id = entity.Id,
            Email = entity.Email,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };

        public static User ToEntity(this UserCreateDto dto) => new User
        {
            Email = dto.Email,
            Password = dto.Password,
        };

        public static IEnumerable<UserReadDto> ToReadDtos(this IEnumerable<User> entities) =>
            entities.Select(e => e.ToReadDto());
    }
}
