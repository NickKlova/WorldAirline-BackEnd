using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldAirlineServer.Models.Auth.Request
{
    public class UpdateRefreshToken
    {
        public string Login { get; set; }
        public string RefreshToken { get; set; }
    }
}
