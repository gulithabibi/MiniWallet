using MiniWalletApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Repositories.Interfaces
{
    public interface IWithdrawalRespository
    {
        Task<IEnumerable<Withdrawal>> Find();
        Task<Withdrawal> FindByID(Guid id);
        Task<Withdrawal> Create(Withdrawal deposit);
        Task<Withdrawal> Update(Withdrawal deposit);
        Task Delete(Guid id);
    }
}