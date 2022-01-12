using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Request
{
    public class ReceivedBuyTicket
    {
        public int Seat { get; set; }
        public string TravelClass { get; set; }
        public int BaggageWeight { get; set; }
        public int TicketSchemeId { get; set; }
    }
}
