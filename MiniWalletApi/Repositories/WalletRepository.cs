using Microsoft.EntityFrameworkCore;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories.Interfaces;
using MiniWalletApi.Constans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniWalletApi.Dtos;
using System.Net;

namespace MiniWalletApi.Repositories
{
    public class WalletRepository : IWalletRespository
    {
        private readonly MiniWalletContext _context;


        public WalletRepository(MiniWalletContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wallet>> Find()
        {
            try
            {
                var wallets = _context.Wallets.ToList();
                return wallets;
            }
            catch (Exception)
            {
                throw new System.NotImplementedException();
            }
        }

        public async Task<Wallet> FindByID(Guid id)
        {
            try
            {
                var customer = _context.Wallets.Where(x => x.Id == id).FirstOrDefault();
                return customer;
            }
            catch (Exception)
            {
                throw new System.NotImplementedException();
            }
        }

        public async Task<Wallet> FindByOwner(Guid custId)
        {
            try
            {
                var wallet = _context.Wallets.Where(x => x.OwnedBy== custId).FirstOrDefault();
                return wallet;
            }
            catch (Exception)
            {
                throw new System.NotImplementedException();
            }
        }

        public async Task<Wallet> FindByToken(string token)
        {
            try
            {
                var wallet = new Wallet();
                var auth = _context.PersonalAccessTokens.Where(x => x.Token == token).FirstOrDefault();
                if (auth != null)
                {
                    wallet = _context.Wallets.Where(x => x.OwnedBy == auth.Id).FirstOrDefault();
                }
                return wallet;
            }
            catch (Exception ex)
            {
                throw new System.NotImplementedException();
            }
        }

        public async Task<decimal> GetBalance(string auth)
        {
            try
            {
                decimal balance = 0;

                var custRepo = new CustomerRepository(_context);
                var customer = custRepo.FinfByToken(auth).Result;
                if (customer == null) return balance;

                var wallet = _context.Wallets.Where(x => x.OwnedBy == customer.Id).FirstOrDefault();
                return wallet.Balance;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseApiResponse> EnableWallet(string token)
        {
            try
            {

                var custRepo = new CustomerRepository(_context);
                var customer = custRepo.FinfByToken(token).Result;
                if (customer == null) return BaseApiResponse.GetUnauthorizedResponse();

                Wallet wallet = _context.Wallets.Where(x => x.OwnedBy == customer.Id).FirstOrDefault();
                if (wallet == null) return BaseApiResponse.GetNotFoundResponse("Wallet not found");

                if (wallet.Status == CommonConstant.WalletStatus.Enabled) 
                    return BaseApiResponse.GetBadRequstResponse("Status wallet already enabled");

                wallet.Status = CommonConstant.WalletStatus.Enabled;
                wallet = await Update(wallet);
              
                return BaseApiResponse.GetCreatedResponse(wallet);
            }
            catch(Exception ex)
            {
                return BaseApiResponse.GetErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseApiResponse> DisableWallet(DisableWalletRqDto value)
        {
            try
            {
                //Check authorized
                var custRepo = new CustomerRepository(_context);
                var customer = custRepo.FinfByToken(value.Authorization).Result;
                if (customer == null) return BaseApiResponse.GetUnauthorizedResponse();



                Wallet wallet = _context.Wallets.Where(x => x.OwnedBy == customer.Id).FirstOrDefault();
                if (wallet == null) return BaseApiResponse.GetNotFoundResponse("Wallet not found");

                if (value.Is_disabled)
                {
                    wallet.Status = CommonConstant.WalletStatus.Disabled;
                    wallet = await Update(wallet);
                }

                return BaseApiResponse.GetCreatedResponse(wallet);
            }
            catch (Exception ex)
            {
                return BaseApiResponse.GetErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseApiResponse> ViewBalance(string token)
        {
            try
            {
                var custRepo = new CustomerRepository(_context);
                var customer = custRepo.FinfByToken(token).Result;
                if (customer == null) return BaseApiResponse.GetUnauthorizedResponse();

                Wallet wallet = _context.Wallets.Where(x => x.OwnedBy == customer.Id).FirstOrDefault();
                if (wallet == null) return BaseApiResponse.GetNotFoundResponse("Wallet not found");

                return BaseApiResponse.GetOkResponse(wallet);
            }
            catch (Exception ex)
            {
                return BaseApiResponse.GetErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }

        }



        public async Task<BaseApiResponse> AddVirtualMoney(DepositRqDto value)
        {
            try
            {
                //Check authorized
                var access = _context.PersonalAccessTokens.Where(x => x.Token == value.Authorization).FirstOrDefault();
                if (access == null) return BaseApiResponse.GetUnauthorizedResponse();

                //Check reference
                var checkRef = _context.Deposits.Where(x => x.ReferenceID == value.Reference_id).FirstOrDefault();
                if (checkRef != null) return BaseApiResponse.GetBadRequstResponse("Reference ID duplicated");

                var wallet = FindByOwner(access.Id).Result;
                if (wallet.Status == CommonConstant.WalletStatus.Disabled)
                    return BaseApiResponse.GetBadRequstResponse("Currently wallet is disabled.");


                //Add  Deposit
                var deposit = new Deposit()
                {
                    Id = Guid.NewGuid(),
                    DepositedBy = access.Id,
                    Status = HttpStatusConstant.StatusType.Success,
                    DepositedAt = DateTime.UtcNow,
                    Amount = value.Amount,
                    ReferenceID = value.Reference_id
                };
                _context.Deposits.Add(deposit);
                await _context.SaveChangesAsync();

                //update Balance
                wallet.Balance = wallet.Balance + value.Amount;
                await Update(wallet);

                return BaseApiResponse.GetCreatedResponse(deposit);


            }catch(Exception ex)
            {
                return BaseApiResponse.GetErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseApiResponse> UseVirtualMoney(WithdrawalRqDto value)
        {
            try
            {
                //Check authorized
                var access = _context.PersonalAccessTokens.Where(x => x.Token == value.Authorization).FirstOrDefault();
                if (access == null) return BaseApiResponse.GetUnauthorizedResponse();

                //check refernce
                var checkRef = _context.Withdrawals.Where(x => x.ReferenceID == value.Reference_id).FirstOrDefault();
                if (checkRef != null) return BaseApiResponse.GetBadRequstResponse("Reference ID duplicated");

                var wallet = FindByOwner(access.Id).Result;
                if (wallet.Status == CommonConstant.WalletStatus.Disabled)
                    return BaseApiResponse.GetBadRequstResponse("Currently wallet is disabled.");


                if (wallet.Balance < value.Amount)
                    return BaseApiResponse.GetBadRequstResponse("Balance not enough");

                //Add withdrawal
                var withdrawal = new Withdrawal()
                {
                    Id = Guid.NewGuid(),
                    WithdrawnBy = access.Id,
                    Status = HttpStatusConstant.StatusType.Success,
                    WithdrawnAt = DateTime.UtcNow,
                    Amount = value.Amount,
                    ReferenceID = value.Reference_id
                };
                _context.Withdrawals.Add(withdrawal);
                await _context.SaveChangesAsync();

                //update balance
                wallet.Balance = wallet.Balance - value.Amount;
                await Update(wallet);
                
                return BaseApiResponse.GetCreatedResponse(withdrawal);
            }
            catch (Exception ex)
            {
                return BaseApiResponse.GetErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Wallet> Create(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            return wallet;
        }

        public async Task<Wallet> Update(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.Entry(wallet).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return wallet;
        }

        public async Task Delete(Guid id)
        {

            var wallet = await _context.Wallets.FindAsync(id);
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
        }

    }
}
