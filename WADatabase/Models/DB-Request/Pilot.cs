using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase.Models.DB_Request
{
    public partial class Pilot
    {
        public Pilot()
        {
            Crews = new HashSet<Crew>();
        }

        public int Id { get; set; }
        public int FlyingHours { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<Crew> Crews { get; set; }
    }
}
