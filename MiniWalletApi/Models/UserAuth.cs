using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Models
{
    public class UserAuth
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime? LastUsedAt { get; set; }
       
    }
}
