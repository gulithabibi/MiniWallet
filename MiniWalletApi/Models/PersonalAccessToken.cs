using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Models
{
    public class PersonalAccessToken
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public string Token { get; set; }
        public DateTime LastUsedAt { get; set; }
    }

}
