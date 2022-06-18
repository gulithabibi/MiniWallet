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
using MiniWalletApi.Libraries;

namespace MiniWalletApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {

        private readonly IWalletRespository _repository;

        public WalletsController(IWalletRespository repository)
        {
            _repository = repository;

        }    

        //POST api/Wallets
        [HttpGet]
        public ActionResult Post()
        {
            try
            {
                string token = "6b3f7dc70abe8aed3e56658b86fa508b472bf238";
                var wallet = _repository.FindByToken(token).Result;
                if (wallet == null)
                {
                    return NotFound();
                }
                return Ok(wallet);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        //GET api/Wallets/desposits
        [HttpPost()]
        [Route("deposits")]
        public ActionResult<Customer> Deposits(Guid id)
        {
            try
            {
                var customer = _repository.FindByID(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //GET api/Wallets/5
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
