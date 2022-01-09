using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment.Interfaces
{
    public interface IAirport
    {
        public Task<Models.API.Response.ReturnAirport> GetAirportAsync(int id);
        public Task<IEnumerable<Models.API.Response.ReturnAirport>> GetAirportsAsync(string country);
        public Task<IEnumerable<Models.API.Response.ReturnAirport>> GetAllAirportsAsync();
        public Task CreateAirportAsync(Models.API.Request.ReceivedAirport incomingData);
        public Task DeleteAirportAsync(int id);
    }
}
