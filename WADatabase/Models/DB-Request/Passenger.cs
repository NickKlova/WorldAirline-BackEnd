using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase.Models.DB_Request
{
    public partial class Passenger
    {
        public Passenger()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PassportSeries { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
