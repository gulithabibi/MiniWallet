using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Models
{
    public class Withdrawal
    {
        public Guid Id { get; set; }
        public Guid WithdrawnBy { get; set; }
        public string Status { get; set; }
        public DateTime WithdrawnAt { get; set; }
        public Decimal? Amount { get; set; }
        public Guid ReferenceID { get; set; }
    }
}
