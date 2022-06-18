using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories;
using MiniWalletApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiniWalletApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {

        //private CustomersRepository _custRepository = new CustomersRepository();
        private readonly ICustomerRespository _repository;

        public CustomersController(ICustomerRespository repository)
        {
            _repository = repository;

        }
        public IActionResult Index()
        {
           // SqlConnection conn = SqlConnection("");

            return Ok();
        }

        //GET api/customers
        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var cust = _repository.Find().Result;
                if (cust == null)
                {
                    return NotFound();
                }
                return Ok(cust);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
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
