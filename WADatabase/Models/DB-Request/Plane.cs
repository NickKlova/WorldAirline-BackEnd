using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase.Models.DB_Request
{
    public partial class Plane
    {
        public Plane()
        {
            TicketSchemes = new HashSet<TicketScheme>();
        }

        public int Id { get; set; }
        public string Model { get; set; }
        public string Number { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int LifeTime { get; set; }
        public bool Ok { get; set; }

        public virtual ICollection<TicketScheme> TicketSchemes { get; set; }
    }
}
