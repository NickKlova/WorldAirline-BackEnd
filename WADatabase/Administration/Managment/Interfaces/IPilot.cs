using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment.Interfaces
{
    public interface IPilot
    {
        public Task<Models.API.Response.ReturnPilot> GetPilotAsync(string login);
        public Task<IEnumerable<Models.API.Response.ReturnPilot>> GetPilotByPersonalInfo(string name, string surname);
        public Task CreatePilotAsync(Models.API.Request.ReceivedPilot incomingData);
        public Task UpdateFlyingHoursAsync(int amount, string login);
        public Task DeletePilotAsync(int id);
    }
}
