using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Request
{
    public class ReceivedWay
    {
        public TimeSpan FlightDuration { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public bool Actual { get; set; }
    }
}
