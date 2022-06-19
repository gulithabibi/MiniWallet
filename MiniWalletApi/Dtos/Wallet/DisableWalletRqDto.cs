using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Dtos
{
    public class DisableWalletRqDto
    {
        public bool Is_disabled { get; set; }
        public string Authorization { get; set; }
    }

}
