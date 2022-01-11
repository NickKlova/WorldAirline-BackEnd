using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Response.ReturnTicket
{
    public class TicketShortInfo
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public int Seat { get; set; }
        public string TravelClass { get; set; }
        public bool Booked { get; set; }
        public decimal Price { get; set; }
        public bool Canceled { get; set; }
    }
}
