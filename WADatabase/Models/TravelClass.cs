using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase
{
    public partial class TravelClass
    {
        public TravelClass()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string ClassName { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
