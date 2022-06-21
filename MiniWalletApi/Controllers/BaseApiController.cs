using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniWalletApi.Controllers
{
    [ApiController]
    public abstract class BaseApiController : BaseController
    {

        CustomerRepository customerRepository;

        protected IActionResult GetResult(BaseApiResponse  response)
        {

            if(response.HttpStatus== HttpStatusCode.OK)
            {
                return Ok(response);
            }
            if (response.HttpStatus == HttpStatusCode.Created)
            {
                return Created("",response);
            }
            if (response.HttpStatus == HttpStatusCode.BadRequest)
            {
                return BadRequest(response);
            }
            if (response.HttpStatus == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }
            if (response.HttpStatus == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(response);
            }
            else
            {
                return BadRequest(response);
            }   
        }

        protected User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

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
    }
}
