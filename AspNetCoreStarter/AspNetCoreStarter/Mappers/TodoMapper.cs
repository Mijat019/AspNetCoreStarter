using AspNetCoreStarter.Dtos;
using AspNetCoreStarter.Models;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreStarter.Mappers
{
    public static class TodoMapper
    {
        public static TodoReadDto ToReadDto(this Todo entity) => new TodoReadDto
        {
            Id = entity.Id,
            Text = entity.Text,
            Checked = entity.Checked,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };

        public static Todo ToEntity(this TodoCreateDto dto) => new Todo
        {
            Checked = dto.Checked,
            Text = dto.Text,
        };

        public static IEnumerable<TodoReadDto> ToReadDtos(this IEnumerable<Todo> entities) =>
            entities.Select(e => e.ToReadDto());
    }
}
