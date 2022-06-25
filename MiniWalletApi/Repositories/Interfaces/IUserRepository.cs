using MiniWalletApi.Dtos;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> Find();
        Task<User> FindByID(Guid id);
        Task<User> FindByUsernamePwd(string username, string password);
        bool IsUserExist(User user);
        Task<User> Create(User user);
        Task<User> Update(User user);
        Task Delete(Guid id);

    }
}