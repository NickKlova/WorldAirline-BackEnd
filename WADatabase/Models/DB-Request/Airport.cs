using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase
{
    public partial class Airport
    {
        public Airport()
        {
            WayArrivalAirports = new HashSet<Way>();
            WayDepartureAirports = new HashSet<Way>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<Way> WayArrivalAirports { get; set; }
        public virtual ICollection<Way> WayDepartureAirports { get; set; }
    }
}
