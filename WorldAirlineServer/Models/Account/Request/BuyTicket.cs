using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WADatabase.Models.API.Request;

namespace WorldAirlineServer.Models.Account.Request
{
    public class BuyTicket
    {
        public ReceivedPassenger Passenger { get; set; }
        public ReceivedBuyTicket Info { get; set; }
    }
}
