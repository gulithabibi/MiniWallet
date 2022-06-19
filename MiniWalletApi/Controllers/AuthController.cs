using Ajax;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniWalletApi.Dtos;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories;
using MiniWalletApi.Repositories.Interfaces;
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
    public class AuthController : BaseController
    {

        //private CustomersRepository _custRepository = new CustomersRepository();
        private readonly ICustomerRespository _repository;

        public AuthController(ICustomerRespository repository)
        {
            _repository = repository;

        }
        
        //GET api/customers
        [HttpPost]
        [Route("api/v1/init")]
        public IActionResult Init(InitRqDto value)
        {
            try
            {
                var resp = _repository.GetInit(value.Customer_xid).Result;
                if (resp == null)
                {
                    return CustomResult(HttpStatusCode.NotFound);
                }

                return CustomResult(resp,HttpStatusCode.Created);

            }
            catch(Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
