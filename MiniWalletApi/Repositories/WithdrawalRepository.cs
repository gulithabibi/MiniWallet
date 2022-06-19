using Microsoft.EntityFrameworkCore;
using MiniWalletApi.Constans;
using MiniWalletApi.Dtos;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Repositories
{
    public class WithdrawalRepository : IWithdrawalRespository
    {
        private readonly MiniWalletContext _context;


        public WithdrawalRepository(MiniWalletContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Withdrawal>> Find()
        {
            return _context.Withdrawals.ToList();
        }

        public async Task<Withdrawal> FindByID(Guid id)
        {
            return await _context.Withdrawals.FindAsync(id);
        }

        public async Task<Withdrawal> Create(WithdrawalRqDto req)
        {
            var withdrawal = new Withdrawal()
            {
                Id = Guid.NewGuid(),
                WithdrawnBy = req.CustomerId,
                Status = HttpStatusConstant.StatusType.Success,
                WithdrawnAt = DateTime.UtcNow,
                Amount = req.Amount,
                ReferenceID = req.Reference_id
            };

            _context.Withdrawals.Add(withdrawal);
            await _context.SaveChangesAsync();

            return withdrawal;
        }

        public async Task<Withdrawal> Update(Withdrawal withdrawal)
        {
            _context.Withdrawals.Update(withdrawal);
            _context.Entry(withdrawal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return withdrawal;
        }

        public async Task Delete(Guid id)
        {

            var withdrawal = await _context.Withdrawals.FindAsync(id);
            _context.Withdrawals.Remove(withdrawal);
            await _context.SaveChangesAsync();
        }
    }
}
