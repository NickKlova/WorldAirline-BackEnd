using System;
using System.Collections.Generic;

#nullable disable

namespace WADatabase
{
    public partial class Ticket
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? TicketSchemeId { get; set; }
        public int Seat { get; set; }
        public int? TravelClassId { get; set; }
        public int? BaggageWeight { get; set; }
        public decimal Price { get; set; }
        public bool Booked { get; set; }
        public int? PassengerId { get; set; }
        public int? AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Passenger Passenger { get; set; }
        public virtual TicketScheme TicketScheme { get; set; }
        public virtual TravelClass TravelClass { get; set; }
    }
}
