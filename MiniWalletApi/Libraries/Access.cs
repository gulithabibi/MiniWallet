using MiniWalletApi.Models;
using System;
using System.Linq;

namespace MiniWalletApi.Libraries
{
    public class Access
    {
        private readonly MiniWalletContext _context;


        public Access(MiniWalletContext context)
        {
            _context = context;
        }

        public Customer Auth(string token)
        {

            return GetAuth(token);
        }

        public Customer GetAuth(string token)
        {
            var cust = new Customer();

            var paToken = _context.PersonalAccessTokens.Where(x => x.Token == token).FirstOrDefault();
            if (paToken != null)
            {
                cust = _context.Customers.Where(x => x.Id == paToken.Id).FirstOrDefault();
            }
            return cust;

        }
    }
}
