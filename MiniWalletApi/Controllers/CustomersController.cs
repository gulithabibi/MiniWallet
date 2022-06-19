using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniWalletApi.Libraries;
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
    [Route("api/v1/[controller]")]
    public class CustomersController : BaseApiController
    {

        //private CustomersRepository _custRepository = new CustomersRepository();
        private readonly ICustomerRespository _repository;

        public CustomersController(ICustomerRespository repository)
        {
            _repository = repository;

        }

        //GET api/customers
        [HttpGet]
        //[Authorize]
        public IActionResult GetAll()
        {
                var resp = _repository.Find().Result;                
                return GetResult(resp);
        }

        //GET api/customers/5
        [HttpGet("{id}")]
        public ActionResult <Customer> GetByID(Guid id)
        {
            try
            {
                var customer = _repository.FindByID(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);

            }catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<ActionResult<List<Customer>>> Add()
        {
            return Ok();
        }
    }
}
