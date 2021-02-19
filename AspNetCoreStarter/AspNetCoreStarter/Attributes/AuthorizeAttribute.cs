using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using AspNetCoreStarter.Contracts.Enums;
using AspNetCoreStarter.Exceptions;
using AspNetCoreStarter.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreStarter.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private HttpContext _httpContext;

        private Role[] _requiredRoles;

        public AuthorizeAttribute(params Role[] minimalRole)
        {
            _requiredRoles = minimalRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _httpContext = context.HttpContext;

            string token = _httpContext.Request
                 .Headers["Authorization"]
                 .FirstOrDefault()?
                 .Split(" ")
                 .Last();

            if (token == null) throw new BusinessException("Token is missing", 401);

            Role role;

            try
            {
                role = this.ValidateToken(token);
            }
            catch
            {
                throw new BusinessException("Token is invalid", 401);
            }

            if (_requiredRoles.Any() && !_requiredRoles.Contains(role))
            {
                throw new BusinessException("You don't have the right permission", 403);
            }
        }

        private Role ValidateToken(string token)
        {
            AppSettings appSettings = _httpContext.RequestServices.GetService(typeof(AppSettings)) as AppSettings;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(appSettings.ServerSecret);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

            string userId = jwtToken.Claims.First(x => x.Type == "id").Value;
            string role = jwtToken.Claims.First(x => x.Type == "role").Value;

            _httpContext.Items["id"] = userId;
            _httpContext.Items["role"] = role;

            return (Role)Enum.Parse(typeof(Role), role);
        }
    }
}
