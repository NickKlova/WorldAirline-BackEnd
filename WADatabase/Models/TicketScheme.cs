using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase
{
    public partial class TicketScheme
    {
        public TicketScheme()
        {
            Crews = new HashSet<Crew>();
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public int? WayId { get; set; }
        public int? PlaneId { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public bool Canceled { get; set; }

        public virtual Plane Plane { get; set; }
        public virtual Way Way { get; set; }
        public virtual ICollection<Crew> Crews { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
