using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Response
{
    public class ReturnAirport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ReturnLocation Location { get; set; }
    }
}
