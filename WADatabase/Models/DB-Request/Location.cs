using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase.Models.DB_Request
{
    public partial class Location
    {
        public Location()
        {
            Airports = new HashSet<Airport>();
        }

        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Airport> Airports { get; set; }
    }
}
