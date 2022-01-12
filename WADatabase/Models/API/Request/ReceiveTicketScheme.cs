using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Request
{
    public class ReceiveTicketScheme
    {
        public int? WayId { get; set; }
        public int? PlaneId { get; set; }
        public DateTime DepartureDate { get; set; }
        public bool Canceled { get; set; }
    }
}
