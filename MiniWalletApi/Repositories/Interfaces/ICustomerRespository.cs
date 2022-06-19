using MiniWalletApi.Dtos;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Repositories.Interfaces
{
    public interface ICustomerRespository
    {
        Task<List<Customer>> Find();
        Task<Customer> FindByID(Guid id);
        Task<Customer> FinfByToken(string token);
        Task<BaseApiResponse> GetInit(Guid custId);
    }
}