﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xTask.Core.Interfaces
{
    public interface ITokenClaimsService
    {
        Task<string> GetTokenAsync(string userName, DateTime expires);
    }
}
