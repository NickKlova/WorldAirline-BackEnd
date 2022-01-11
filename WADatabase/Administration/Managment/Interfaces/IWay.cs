using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment.Interfaces
{
    public interface IWay
    {
        public Task<IEnumerable<Models.API.Response.ReturnWay>> GetAllWaysAsync();
        public Task<Models.API.Response.ReturnWay> GetWayAsync(int? id);
        public Task CreateWayAsync(Models.API.Request.ReceivedWay incomingData);
        public Task ChangeWayActualityAsync(int wayId, bool actuality);
        public Task DeleteWayAsync(int wayId);
    }
}
