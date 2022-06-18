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
       
    }
}
