﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Dtos
{
    public class WithdrawalRqDto
    {
        public Decimal Amount { get; set; }
        public Guid Reference_id { get; set; }
        public string Authorization { get; set; }
    }
}