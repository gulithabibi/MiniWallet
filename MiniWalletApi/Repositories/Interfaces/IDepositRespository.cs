using MiniWalletApi.Dtos;
using MiniWalletApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Repositories.Interfaces
{
    public interface IDepositRespository
    {
        Task<IEnumerable<Deposit>> Find();
        Task<Deposit> FindByID(Guid id);
        Task<Deposit> Create(DepositRqDto req);
        Task<Deposit>Update(Deposit deposit);
        Task Delete(Guid id);
    }
}