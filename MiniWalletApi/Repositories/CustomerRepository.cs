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
    public class CustomerRepository : ICustomerRespository
    {
        private readonly MiniWalletContext _context;


        public CustomerRepository(MiniWalletContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> Find()
        {
            try
            {
                var customers = _context.Customers.ToList();
                return customers;
            }
            catch (Exception)
            {
                throw new System.NotImplementedException();
            }
        }

        public async Task<Customer> FindByID(Guid id)
        {
            try
            {
                var customer = _context.Customers.Where(x => x.Id == id).FirstOrDefault();
                return customer;
            }
            catch (Exception)
            {
                throw new System.NotImplementedException();
            }
        } 



        public async Task<Customer> FinfByToken(string token)
        {
            try
            {
                var cust = new Customer();
                var auth = _context.PersonalAccessTokens.Where(x => x.Token == token).FirstOrDefault();
                if (auth != null)
                {
                    cust = _context.Customers.Where(x => x.Id == auth.Id).FirstOrDefault();
                }

                return cust;
            }
            catch (Exception)
            {
                throw new System.NotImplementedException();
            }

        }

        public async Task<InitRsDto> GetInit(Guid custId)
        {
            try
            {
                InitRsDto resp = null;

                var accessToken = _context.PersonalAccessTokens.Where(x => x.Id == custId).FirstOrDefault();
                if (accessToken != null) {
                    _context.PersonalAccessTokens.Remove(accessToken);
                    await _context.SaveChangesAsync();
                }

                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                accessToken = new PersonalAccessToken()
                {
                    Id = custId,
                    Name = "MiniWalletToken",
                    Token = token,
                    LastUsedAt = DateTime.UtcNow
                };
                _context.PersonalAccessTokens.Add(accessToken);
                await _context.SaveChangesAsync();

                resp = new InitRsDto();
                resp.Token = token;

                return resp;

            }
            catch(Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
       
    }
}
