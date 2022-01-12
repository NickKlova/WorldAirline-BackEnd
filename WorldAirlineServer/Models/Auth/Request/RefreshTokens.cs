using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldAirlineServer.Models.Auth.Request
{
    public class RefreshTokens
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
