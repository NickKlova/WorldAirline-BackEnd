using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Response
{
    public class ReturnPilotCrew
    {
        public ReturnPilot pilot { get; set; }
        public ReturnPosition position { get; set; }
    }
}
