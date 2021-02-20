using AspNetCoreStarter.Attributes;
using AspNetCoreStarter.Dtos;
using AspNetCoreStarter.Helpers;
using AspNetCoreStarter.Services;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [Authorize(Policies.Admin)]
        [HttpGet("{id}")]
        public ActionResult<UserReadDto> Get(int id)
        {
            UserReadDto result = _service.Get(id);

            return Ok(result); 
        }

        [Authorize(Policies.Admin)]
        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> Get()
        {
            IEnumerable<UserReadDto> result = _service.Get();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile-data")]
        public ActionResult<UserReadDto> GetProfileData()
        {
            int userId = Convert.ToInt32(HttpContext.Items["id"]);

            UserReadDto result = _service.Get(userId);

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<UserReadDto> Create(UserCreateDto dto)
        {
            UserReadDto result = _service.Create(dto);

            return Ok(result);
        }

        [Authorize(Policies.Admin)]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok();
        }
    }
}
