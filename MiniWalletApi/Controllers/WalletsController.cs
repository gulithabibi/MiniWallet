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
    public class WalletsController : BaseController
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
            try
            {

                var token = Request.Headers["Authorization"];

                var wallet = _repository.EnableWallet(token).Result;
                if (wallet == null)
                {
                    return CustomResult("Status wallet already enabled",HttpStatusCode.BadRequest);
                }
                return CustomResult(wallet,HttpStatusCode.Created);
            }
            catch(Exception ex)
            {
                return CustomResult(ex.Message,HttpStatusCode.InternalServerError);
            }
        }

        //POST api/Wallets
        [HttpGet]
        //[Authorize]
        [Route("api/v1/wallet")]
        public IActionResult ViewBalance()
        {
            try
            {
                var token = Request.Headers["Authorization"];

                var wallet = _repository.ViewBalance(token).Result;
                if (wallet == null)
                {
                    return CustomResult(HttpStatusCode.NotFound);
                }
                return CustomResult(wallet);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        //GET api/Wallets/desposits
        [HttpPost()]
        [Route("api/v1/wallet/deposits")]
        public IActionResult AddVirtualMoney(DepositRqDto value)
        {
            try
            {
                
                value.Authorization= Request.Headers["Authorization"];

                //check wallet enable
                if (!isEnable(value.Authorization))
                {
                    return CustomResult("Currently wallet is disabled.", HttpStatusCode.BadRequest);
                }

                var deposit = _repository.AddVirtualMoney(value).Result;
                if (deposit == null)
                {
                    return CustomResult("Reference ID duplicated",HttpStatusCode.BadRequest);
                }
                return CustomResult(deposit,HttpStatusCode.Created);

            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        //GET api/Wallets/withdrawals
        [HttpPost()]
        [Route("api/v1/wallet/withdrawals")]
        public IActionResult UseVirtualMoney(WithdrawalRqDto value)
        {
            try
            {
                value.Authorization = Request.Headers["Authorization"];

                //check wallet enable
                if (!isEnable(value.Authorization))
                {
                    return CustomResult("Currently wallet is disabled.", HttpStatusCode.BadRequest);
                }

                //check balance
                var balance = _repository.GetBalance(value.Authorization).Result;
                if (balance < value.Amount)
                    return CustomResult("Balance not enough", HttpStatusCode.BadRequest);
                

                var deposit = _repository.UseVirtualMoney(value).Result;
                if (deposit == null)
                {
                    return CustomResult("Reference ID duplicated", HttpStatusCode.BadRequest);
                }
                return CustomResult(deposit,HttpStatusCode.Created);

            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        //GET api/v1/Wallets/
        [HttpPatch()]
        [Route("api/v1/wallet")]
        public IActionResult DisablewWallet(DisableWalletRqDto value)
        {
            try
            {
                value.Authorization = Request.Headers["Authorization"];

                var wallet = _repository.FindByToken(value.Authorization).Result;
                if (wallet == null) return CustomResult(HttpStatusCode.BadRequest);


                if (value.Is_disabled)
                {
                    wallet.Status = CommonConstant.WalletStatus.Disabled;
                    wallet=_repository.Update(wallet).Result;
                }

                return CustomResult(wallet);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }


        private bool isEnable(string auth)
        {
            var wallet = _repository.FindByToken(auth).Result;
            if (wallet.Status != CommonConstant.WalletStatus.Enabled)
            {
                return false;
            }

            return true;

        }


    }
}
