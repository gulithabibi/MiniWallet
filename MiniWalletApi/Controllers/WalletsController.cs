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
using Microsoft.AspNetCore.Authorization;
using CoreApiResponse;
using MiniWalletApi.Dtos;
using MiniWalletApi.Constans;

namespace MiniWalletApi.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    public class WalletsController : BaseApiController
    {

        private readonly IWalletRespository _repository;

        public WalletsController(IWalletRespository repository)
        {
            _repository = repository;

        }    

        //POST api/Wallets
        [HttpPost]
        //[Authorize]
        [Route("api/v1/wallet")]
        public IActionResult EnableWallet()
        {
            var token = Request.Headers["Authorization"];

            var resp = _repository.EnableWallet(token).Result;
            return GetResult(resp);
        }

        //POST api/Wallets
        [HttpGet]
        //[Authorize]
        [Route("api/v1/wallet")]
        public IActionResult ViewBalance()
        {
            var token = Request.Headers["Authorization"];
            var wallet = _repository.ViewBalance(token).Result;
            return GetResult(wallet);
        }

        //GET api/Wallets/desposits
        [HttpPost()]
        [Route("api/v1/wallet/deposits")]
        public IActionResult AddVirtualMoney(DepositRqDto value)
        {
                value.Authorization= Request.Headers["Authorization"];
                var resp = _repository.AddVirtualMoney(value).Result;
            return GetResult(resp);
        }

        //GET api/Wallets/withdrawals
        [HttpPost()]
        [Route("api/v1/wallet/withdrawals")]
        public IActionResult UseVirtualMoney(WithdrawalRqDto value)
        {
            value.Authorization = Request.Headers["Authorization"];

            var resp = _repository.UseVirtualMoney(value).Result;
            return GetResult(resp);

        }

        //GET api/v1/Wallets/
        [HttpPatch()]
        [Route("api/v1/wallet")]
        public IActionResult DisablewWallet(DisableWalletRqDto value)
        {
            value.Authorization = Request.Headers["Authorization"];

            var resp = _repository.DisableWallet(value).Result;
            return GetResult(resp);
        }

    }
}
