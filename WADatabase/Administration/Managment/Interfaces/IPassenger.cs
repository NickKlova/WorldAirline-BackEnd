using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment.Interfaces
{
    public interface IPassenger
    {
        public Task<IEnumerable<Models.API.Response.ReturnPassenger>> GetPassengersBySurnameAsync(string surname);
        public Task<Models.API.Response.ReturnPassenger> GetPassengerAsync(int? id);
        public Task CreatePassengerAsync(Models.API.Request.ReceivedPassenger incomingData);
        public Task DeletePassengerAsync(int id);
    }
}
