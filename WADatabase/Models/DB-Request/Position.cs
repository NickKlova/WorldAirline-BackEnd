using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase.Models.DB_Request
{
    public partial class Position
    {
        public Position()
        {
            Crews = new HashSet<Crew>();
        }

        public int Id { get; set; }
        public string PositionName { get; set; }

        public virtual ICollection<Crew> Crews { get; set; }
    }
}
