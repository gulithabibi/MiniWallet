using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid OwnedBy { get; set; }
        public string Status { get; set; }
        public DateTime EnabledAt { get; set; }
        public Decimal Balance { get; set; }
    }
}
