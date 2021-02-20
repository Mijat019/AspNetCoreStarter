using AspNetCoreStarter.Attributes;
using AspNetCoreStarter.Contracts.Enums;
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
    public class TodosController : ControllerBase
    {
        private readonly TodoService _service;

        public TodosController(TodoService service)
        {
            _service = service;
        }

        [Authorize(Policies.UserAdmin)]
        [HttpGet("{id}")]
        public ActionResult<TodoReadDto> Get(int id)
        {
            TodoReadDto result = _service.Get(id);

            return Ok(result); 
        }

        [Authorize(Policies.Admin)]
        [HttpGet]
        public ActionResult<IEnumerable<TodoReadDto>> Get()
        {
            IEnumerable<TodoReadDto> result = _service.Get();

            return Ok(result);
        }

        [Authorize(Policies.UserAdmin)]
        [HttpGet("for-user")]
        public ActionResult<IEnumerable<TodoReadDto>> GetByUserId()
        {
            int userId = Convert.ToInt32(HttpContext.Items["id"]);

            IEnumerable<TodoReadDto> result = _service.GetByUserId(userId);

            return Ok(result);
        }

        [Authorize(Policies.UserAdmin)]
        [HttpPost]
        public ActionResult<TodoReadDto> Create(TodoCreateDto dto)
        {
            int userId = Convert.ToInt32(HttpContext.Items["id"]);

            TodoReadDto result = _service.Create(userId, dto);

            return Ok(result);
        }

        [Authorize(Policies.User)]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok();
        }
    }
}
