using Microsoft.EntityFrameworkCore;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories.Interfaces;
using MiniWalletApi.Constans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<Wallet> EnableWallet(string token)
        {
            Wallet wallet=null;
            ICustomerRespository custRepo = new CustomerRepository(_context);
            var customer = custRepo.FinfByToken(token).Result;
            if (customer == null) return wallet;

            wallet = _context.Wallets.Where(x => x.OwnedBy == customer.Id).FirstOrDefault();
            if (wallet == null) return wallet;

            if (wallet.Status !=Common.WalletStatus.Enabled)
            {
                wallet.Status = Common.WalletStatus.Enabled;
                wallet=await Update(wallet);
            }

            return wallet;
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
