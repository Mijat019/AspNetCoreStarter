using AspNetCoreStarter.Exceptions;
using AspNetCoreStarter.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace AspNetCoreStarter.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            this.VerifyToken(context);

            string id = context.HttpContext.Items["id"] as string;

            if (id == null) throw new BusinessException("Unauthenticated", 401);
        }

        private void VerifyToken(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request
                 .Headers["Authorization"]
                 .FirstOrDefault()?
                 .Split(" ")
                 .Last();

            if (token == null) return;

            this.ValidateToken(context.HttpContext, token);

        }

        private void ValidateToken(HttpContext httpContext, string token)
        {
            AppSettings appSettings = httpContext.RequestServices.GetService(typeof(AppSettings)) as AppSettings;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(appSettings.ServerSecret);

            try
            {
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

                httpContext.Items["id"] = userId;
            }
            catch
            { }
        }
    }
}
