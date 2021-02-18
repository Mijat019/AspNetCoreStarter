using AspNetCoreStarter.Dtos;
using AspNetCoreStarter.Exceptions;
using AspNetCoreStarter.Helpers;
using AspNetCoreStarter.Models;
using AspNetCoreStarter.Util;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspNetCoreStarter.Services
{
    public class AuthService
    {
        private readonly UserService _userService;

        private readonly AppSettings _appSettings;

        public AuthService(UserService userService, AppSettings appSettings)
        {
            _userService = userService;
            _appSettings = appSettings;
        }

        public string Auth(SignInDto dto)
        {
            User user = _userService.Get(dto.Email);

            if (user == null) throw new BusinessException("Incorrect credentials", 400);

            string hashedPassword = HashHelper.Hash(dto.Password);

            if (user.Password != hashedPassword) throw new BusinessException("Incorrect credentials", 400);

            string token = this.GenereateToken(user);

            return token;
        }

        private string GenereateToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(_appSettings.ServerSecret);

            ClaimsIdentity subject = new ClaimsIdentity(
                new[] { new Claim("id", user.Id.ToString()) }
            );

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
