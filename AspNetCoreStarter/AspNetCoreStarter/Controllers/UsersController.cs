using AspNetCoreStarter.Dtos;
using AspNetCoreStarter.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspNetCoreStarter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public UserReadDto Get(int id) => _service.Get(id);

        [HttpGet]
        public IEnumerable<UserReadDto> Get() => _service.Get();

        [HttpPost]
        public UserReadDto Create(UserCreateDto dto) => _service.Create(dto);

        [HttpDelete]
        public void Delete(int id) => _service.Delete(id);
    }
}
