using Microsoft.EntityFrameworkCore;
using MiniWalletApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Libraries
{
    public class MiniWalletContext:DbContext
    {
        public MiniWalletContext(DbContextOptions<MiniWalletContext>opt):base(opt)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
