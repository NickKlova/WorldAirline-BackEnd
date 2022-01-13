using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Response
{
    public class ReturnWay
    {
        public int Id { get; set; }
        public string FlightDuration { get; set; }
        public ReturnAirport DepartureAirport { get; set; }
        public ReturnAirport ArrivalAirport { get; set; }
        public bool Actual { get; set; }
    }
}
