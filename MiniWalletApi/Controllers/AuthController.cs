using Ajax;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniWalletApi.Dtos;
using MiniWalletApi.Filters;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories;
using MiniWalletApi.Repositories.Interfaces;
using MiniWalletApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MiniWalletApi.Controllers
{
    [ApiController]
    //[Route("api/v1/[controller]")]
    public class AuthController : BaseApiController
    {

        //private CustomersRepository _custRepository = new CustomersRepository();
        private readonly ICustomerRespository _repository;
        private readonly IAuthService _service;

        public AuthController(ICustomerRespository repository, IAuthService service)
        {
            _repository = repository;
            _service = service;

        }
        
        //GET api/customers
        [HttpPost]
        [Route("api/v1/init")]
        public IActionResult Init(InitRqDto value)
        {
            var resp = _repository.GetInit(value.Customer_xid).Result;
            return GetResult(resp);
        }

        [HttpPost]
        [Route("api/login")]
        public IActionResult Login([FromBody] UserLoginDto userLogin)
        {
            var resp = _service.Login(userLogin).Result;
            return GetResult(resp);
        }

        [HttpGet]
        [Authorize]
        [TokenAuthenticationFilter]
        [Route("api/logout")]
        public IActionResult Logout()
        {
            var token = GetCurrentToken();
            var resp = _service.Logout(token).Result;
            return GetResult(resp);
        }

        [HttpPost]
        [TokenAuthenticationFilter]
        [Route("api/register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            var resp = _service.Register(registerDto).Result;
            return GetResult(resp);
        }
    }
}
