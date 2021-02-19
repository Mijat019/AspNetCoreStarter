using AspNetCoreStarter.Attributes;
using AspNetCoreStarter.Dtos;
using AspNetCoreStarter.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AspNetCoreStarter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoService _service;

        public TodosController(TodoService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<TodoReadDto> Get(int id)
        {
            TodoReadDto result = _service.Get(id);

            return Ok(result); 
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<TodoReadDto>> Get()
        {
            IEnumerable<TodoReadDto> result = _service.Get();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile-data")]
        public ActionResult<TodoReadDto> GetProfileData()
        {
            int userId = Convert.ToInt32(HttpContext.Items["id"]);

            TodoReadDto result = _service.GetByUserId(userId);

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<TodoReadDto> Create(TodoCreateDto dto)
        {
            int userId = Convert.ToInt32(HttpContext.Items["id"]);

            TodoReadDto result = _service.Create(userId, dto);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok();
        }
    }
}
