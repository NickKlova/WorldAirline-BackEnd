using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment.Interfaces
{
    public interface ICrew
    {
        public Task<IEnumerable<Models.API.Response.ReturnPilotCrew>> GetCrewByTicketSchemeAsync(int ticketId);
        public Task AddPilotToCrewAsync(string login, int schemeId, string pos);
        public Task DeletePilotFromCrewAsync(string login);
        public Task DeleteCrewAsync(int ticketId);
    }
}
