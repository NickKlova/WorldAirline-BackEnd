using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Response.ReturnTicket
{
    public class TicketFullInfo
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public ReturnTicketScheme TicketScheme { get; set; }
        public int Seat { get; set; }
        public ReturnTravelClass TravelClass { get; set; }
        public int? BaggageWeight { get; set; }
        public decimal Price { get; set; }
        public bool Booked { get; set; }
        public ReturnPassenger Passenger { get; set; }
        public ReturnAccount Account { get; set; }
    }
}
