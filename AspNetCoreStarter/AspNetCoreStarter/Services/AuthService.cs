using AspNetCoreStarter.Dtos;
using AspNetCoreStarter.Dtos.User;
using AspNetCoreStarter.Exceptions;
using AspNetCoreStarter.Helpers;
using AspNetCoreStarter.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace AspNetCoreStarter.Services
{
    public class AuthService
    {
        private readonly UserService _userService;

        private readonly JwtSecurityTokenHandler _jwtHandler;
        private readonly JwtHeader _definedHeader;

        public AuthService(UserService userService, IConfiguration configuration)
        {
            _userService = userService;

            _jwtHandler = new JwtSecurityTokenHandler();

            string serverPrivateKey = configuration.GetValue<string>("AppSettings:ServerSecret");

            _definedHeader = new JwtHeader(new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(serverPrivateKey)),
                SecurityAlgorithms.HmacSha256
            ));

        }

        public string GenereateToken(SignInDto dto)
        {
            User found = _userService.Get(dto.Email);

            if (found == null) throw new BusinessException("Credentials incorrect.", 400);
            if (found.Password != HashHelper.Hash(dto.Password)) throw new BusinessException("Credentials incorrect.", 400);

            JwtSecurityToken token = new JwtSecurityToken(_definedHeader, new JwtPayload
            {
                { "id", found.Id },
                { "email", found.Email },
                { "issued at", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ff") },
            });

            return _jwtHandler.WriteToken(token);
        }

        public ClaimsDto Verify(string tokenString)
        {
            JwtSecurityToken tokenFromrequest = _jwtHandler.ReadJwtToken(tokenString);

            JwtSecurityToken token = new JwtSecurityToken(_definedHeader, tokenFromrequest.Payload);

            if (_jwtHandler.WriteToken(token) != tokenString)
                throw new BusinessException("Not authorized.", 401);

            return new ClaimsDto
            {
                Id = tokenFromrequest.Claims.First(e => e.Type == "id").Value,
                Email = tokenFromrequest.Claims.First(e => e.Type == "email").Value
            };
        }
    }
}
