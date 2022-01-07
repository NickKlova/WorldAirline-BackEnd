using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Request
{
    public class ReceivedPlane
    {
        public string Model { get; set; }
        public string Number { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int LifeTime { get; set; }
        public bool Ok { get; set; }
    }
}
