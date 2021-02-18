using AspNetCoreStarter.Dtos;
using AspNetCoreStarter.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreStarter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult Auth(SignInDto dto)
        {
            string token = _service.Auth(dto);

            return Ok(new { token });
        }
    }
}
