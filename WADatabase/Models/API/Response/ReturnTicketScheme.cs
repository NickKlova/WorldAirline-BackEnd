using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Response
{
    public class ReturnTicketScheme
    {
        public ReturnWay Way { get; set; }
        public ReturnPlane Plane { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public bool Canceled { get; set; }
    }
}
