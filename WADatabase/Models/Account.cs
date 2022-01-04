using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase
{
    public partial class Account
    {
        public Account()
        {
            Pilots = new HashSet<Pilot>();
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal? Balance { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Pilot> Pilots { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
