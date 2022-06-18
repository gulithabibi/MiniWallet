using MiniWalletApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Repositories.Interfaces
{
    public interface IWalletRespository
    {
        Task<IEnumerable<Wallet>> Find();
        Task<Wallet> FindByID(Guid id);

        Task<Wallet> FindByOwner(Guid custId);
        Task<Wallet> FindByToken(string token);
        Task<Wallet> Create(Wallet deposit);
        Task<Wallet> Update(Wallet deposit);
        Task Delete(Guid id);
    }
}