using MiniWalletApi.Dtos;
using MiniWalletApi.Libraries;
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

        Task<decimal> GetBalance(string auth);

        Task<BaseApiResponse> EnableWallet(string token);

        Task<BaseApiResponse> DisableWallet(DisableWalletRqDto value);

        Task<BaseApiResponse> ViewBalance(string token);

        Task<BaseApiResponse> AddVirtualMoney(DepositRqDto value);

        Task<BaseApiResponse> UseVirtualMoney(WithdrawalRqDto value);

        Task<Wallet> Create(Wallet deposit);
        Task<Wallet> Update(Wallet deposit);
        Task Delete(Guid id);
    }
}