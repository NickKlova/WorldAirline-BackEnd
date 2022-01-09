using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldAirlineServer.Models.Account
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
