using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase
{
    public partial class Crew
    {
        public int Id { get; set; }
        public int? PilotId { get; set; }
        public int? CrewPositionId { get; set; }
        public int? TicketSchemeId { get; set; }

        public virtual Position CrewPosition { get; set; }
        public virtual Pilot Pilot { get; set; }
        public virtual TicketScheme TicketScheme { get; set; }
    }
}
