﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldAirlineServer.Models.Account.Request
{
    public class AccountLogin
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
