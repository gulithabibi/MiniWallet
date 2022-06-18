using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Models
{
    public class Deposit
    {
        public Guid Id { get; set; }
        public Guid DepositedBy { get; set; }
        public string Status { get; set; }
        public DateTime DepositedAt { get; set; }
        public Decimal Amount { get; set; }
        public Guid ReferenceID { get; set; }
    }
}
