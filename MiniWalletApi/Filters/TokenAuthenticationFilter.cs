using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MiniWalletApi.Constans;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniWalletApi.Filters
{

    //public class TokenAuthenticationAttribute : TypeFilterAttribute
    //{
    //    public TokenAuthenticationAttribute(string role) : base(typeof(TokenAuthenticationFilter))
    //    {
    //        Arguments = new object[] { new Claim(role) };
    //    }
    //}

    public class TokenAuthenticationFilter :Attribute, IAuthorizationFilter
    {
       
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var result = true;
            string message = string.Empty;

            Microsoft.Extensions.Primitives.StringValues authToken;
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out authToken);
            var _token = authToken.FirstOrDefault();
            if (_token == null)
            {
                result = false;
                message = CommonConstant.NegativeMessage.Token.TokenNotValid;
            }


            if (result)
            {
                //var currentUser = GetCurrentUser(context);
                //if (!IsValidRole(currentUser))
                //{
                //    result = false;
                //    message = CommonConstant.NegativeMessage.Token.TokenNotValid;
                //}


                string token = _token.Substring(7);
                var dbcontext = context.HttpContext.RequestServices.GetRequiredService<MiniWalletContext>();
                var userAuth = dbcontext.UserAuths.Where(x => x.Token == token).FirstOrDefault();
                if (userAuth == null)
                {
                    result = false;
                    message = CommonConstant.NegativeMessage.Token.TokenNotValid;
                }
                else if (userAuth.ExpiredAt < DateTime.Now)
                {
                    result = false;
                    message = CommonConstant.NegativeMessage.Token.TokenExpired;
                }
            }

            if (result) return;

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
            context.Result = new JsonResult("Unauthorized")
            {
                Value = new { Status = "Error", Data = new { Message =message } }
            };
            
        }

        private User GetCurrentUser(AuthorizationFilterContext context)
        {
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Fullname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }

            return null;
        }

        private bool IsValidRole(User user)
        {
           

            return true;
        }
    }
}
