using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Request
{
    public class ReceivedTicket
    {
        public int? TicketSchemeId { get; set; }
        public string TravelClass { get; set; }
        public decimal Price { get; set; }
        public int Seat { get; set; }
        public int TicketAmount { get; set; }
    }
}
