using Microsoft.EntityFrameworkCore;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Repositories
{
    public class DepositRepository : IDepositRespository
    {
        private readonly MiniWalletContext _context;


        public DepositRepository(MiniWalletContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Deposit>> Find()
        {
            return _context.Deposits.ToList();
        }

        public async Task<Deposit> FindByID(Guid id)
        {

            return await _context.Deposits.FindAsync(id);
            //try
            //{
            //    var deposit = _context.Deposits.Where(x => x.Id == id).FirstOrDefault();
            //    return deposit;
            //}
            //catch (Exception)
            //{
            //    throw new System.NotImplementedException();
            //}
        }

        public async Task<Deposit> Create(Deposit deposit)
        {
            _context.Deposits.Add(deposit);
            await _context.SaveChangesAsync();

            return deposit;
        }

        public async Task<Deposit> Update(Deposit deposit)
        {
            _context.Deposits.Update(deposit);
            _context.Entry(deposit).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return deposit;
        }

        public async Task Delete(Guid id)
        {

            var deposit = await _context.Deposits.FindAsync(id);
            _context.Deposits.Remove(deposit);
            await _context.SaveChangesAsync();
        }
    }
}
