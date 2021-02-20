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

        private Role _requiredRole;

        public AuthorizeAttribute() 
        {
            _requiredRole = Role.User;
        }

        public AuthorizeAttribute(params Role[] minimalRole)
        {
            _requiredRole = minimalRole[0];
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _httpContext = context.HttpContext;

            string token = this.GetTokenFromRequest();

            Role role = this.ValidateTokenAndGetRole(token);

            this.CheckUsersRole(role);
        }

        private string GetTokenFromRequest()
        {
            string token = _httpContext.Request
                 .Headers["Authorization"]
                 .FirstOrDefault()?
                 .Split(" ")
                 .Last();

            if (token == null) throw new BusinessException("Token is missing", 401);

            return token;
        }

        private Role ValidateTokenAndGetRole(string token)
        {
            try
            {
                Role role = this.ValidateToken(token);

                return role;
            }
            catch
            {
                throw new BusinessException("Token is invalid", 401);
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

        private void CheckUsersRole(Role usersRole)
        {
            int doesContainRole = (int)usersRole & (int)_requiredRole;
            
            if(doesContainRole == 0) throw new BusinessException("Forbidden", 403);
        }

    }
}
