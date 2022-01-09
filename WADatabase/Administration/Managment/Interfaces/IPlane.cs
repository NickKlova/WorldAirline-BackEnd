using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment.Interfaces
{
    public interface IPlane
    {
        public Task<IEnumerable<Models.API.Response.ReturnPlane>> GetAllPlanesAsync();
        public Task<Models.API.Response.ReturnPlane> GetPlaneAsync(int id);
        public Task CreatePlaneAsync(Models.API.Request.ReceivedPlane incomingData);
        public Task DeletePlaneAsync(int id);
    }
}
