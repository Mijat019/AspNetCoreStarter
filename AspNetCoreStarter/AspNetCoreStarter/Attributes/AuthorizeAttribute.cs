using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
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

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _httpContext = context.HttpContext;

            string token = _httpContext.Request
                 .Headers["Authorization"]
                 .FirstOrDefault()?
                 .Split(" ")
                 .Last();

            if (token == null) throw new BusinessException("Token is missing", 401);

            try
            {
                this.ValidateToken(token);
            }
            catch
            {
                throw new BusinessException("Token is invalid", 401);
            }
        }

        private void ValidateToken(string token)
        {
            AppSettings appSettings = _httpContext.RequestServices.GetService(typeof(AppSettings)) as AppSettings;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(appSettings.ServerSecret);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            }, out SecurityToken validatedToken);

            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

            string userId = jwtToken.Claims.First(x => x.Type == "id").Value;

            _httpContext.Items["id"] = userId;
        }
    }
}
