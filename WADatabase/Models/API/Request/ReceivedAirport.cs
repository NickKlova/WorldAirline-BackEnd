using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Request
{
    public class ReceivedAirport
    {
        public string Name { get; set; }
        public ReceivedLocation Location { get; set; }
    }
}
