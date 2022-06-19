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
                ICustomerRespository custRepo = new CustomerRepository(_context);
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

        public async Task<Wallet> EnableWallet(string token)
        {
            try
            {
                Wallet wallet = null;
                ICustomerRespository custRepo = new CustomerRepository(_context);
                var customer = custRepo.FinfByToken(token).Result;
                if (customer == null) return wallet;

                wallet = _context.Wallets.Where(x => x.OwnedBy == customer.Id).FirstOrDefault();
                if (wallet == null) return wallet;


                if (wallet.Status == CommonConstant.WalletStatus.Enabled) return null;




                wallet.Status = CommonConstant.WalletStatus.Enabled;
                wallet = await Update(wallet);
              
                return wallet;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public async Task<Wallet> ViewBalance(string token)
        {
            try
            {
                Wallet wallet = null;
                ICustomerRespository custRepo = new CustomerRepository(_context);
                var customer = custRepo.FinfByToken(token).Result;
                if (customer == null) return wallet;

                wallet = _context.Wallets.Where(x => x.OwnedBy == customer.Id).FirstOrDefault();
                if (wallet == null) return wallet;

                return wallet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        public async Task<Deposit> AddVirtualMoney(DepositRqDto value)
        {
            try
            {
                Deposit deposit = null;
                //get cust
                var access = _context.PersonalAccessTokens.Where(x => x.Token == value.Authorization).FirstOrDefault();
                if (access == null) return deposit;


                var checkRef = _context.Deposits.Where(x => x.ReferenceID == value.Reference_id).FirstOrDefault();
                if (checkRef != null) return deposit;

                //set value
                deposit = new Deposit()
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

                //update balance
                var wallet = FindByOwner(access.Id).Result;
                wallet.Balance = wallet.Balance + value.Amount;
                await Update(wallet);

                return deposit;


            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Withdrawal> UseVirtualMoney(WithdrawalRqDto value)
        {
            try
            {
                Withdrawal withdrawal = null;
                //get cust
                var access = _context.PersonalAccessTokens.Where(x => x.Token == value.Authorization).FirstOrDefault();
                if (access == null) return withdrawal;


                var checkRef = _context.Withdrawals.Where(x => x.ReferenceID == value.Reference_id).FirstOrDefault();
                if (checkRef != null) return withdrawal;

                //set value
                withdrawal = new Withdrawal()
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
                var wallet = FindByOwner(access.Id).Result;
                if (wallet.Balance >= value.Amount)
                {
                    wallet.Balance = wallet.Balance - value.Amount;
                    await Update(wallet);
                }

                return withdrawal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
