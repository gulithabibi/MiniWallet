using MiniWalletApi.Dtos;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Repositories.Interfaces
{
    public interface IUserAuthRepository
    {
        Task<List<UserAuth>> Find();
        Task<UserAuth> FindByID(Guid id);
        Task<UserAuth> FindByToken(string token);
        Task<UserAuth> Create(UserAuth userAuth);
        Task<UserAuth> Update(UserAuth userAuth);
        Task Delete(Guid id);
        Task Delete(string token);

    }
}