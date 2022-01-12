using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Response
{
    public class ReturnPurchasedTicketByCode
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DepartureLocation { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalLocation { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int Seat { get; set; }
        public string TravelClass { get; set; }
        public bool WayIsActual { get; set; }
        public bool Canceled { get; set; }

    }
}
