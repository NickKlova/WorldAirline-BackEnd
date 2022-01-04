using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase
{
    public partial class Way
    {
        public Way()
        {
            TicketSchemes = new HashSet<TicketScheme>();
        }

        public int Id { get; set; }
        public TimeSpan FlightDuration { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public bool Actual { get; set; }

        public virtual Airport ArrivalAirport { get; set; }
        public virtual Airport DepartureAirport { get; set; }
        public virtual ICollection<TicketScheme> TicketSchemes { get; set; }
    }
}
