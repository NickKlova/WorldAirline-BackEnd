using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldAirlineServer.Models.Account.Request
{
    public class ChangeLogin
    {
        public string NewLogin { get; set; }
        public string Password { get; set; }
    }
}
