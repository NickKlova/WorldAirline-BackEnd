using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment.Interfaces
{
    public interface ITicket
    {
        public Task<IEnumerable<Models.API.Response.ReturnTicketScheme>> GetTicketSchemeByWayIdAsync(int wayId);
        public Task<IEnumerable<Models.API.Response.ReturnTicketScheme>> GetTicketSchemeByIdAsync(int id);
        public Task CreateTicketSchemeAsync(Models.API.Request.ReceiveTicketScheme incomingTicketScheme);
        public Task CreateTicketsAsync(int ticketAmount, string travelClass, decimal price, Models.API.Request.ReceivedTicket incomingTicket);
        public Task FlightStatusChangeAsync(int id, bool status);
        public Task FlightStatusChangeAsync(int wayId, DateTime departureDate, bool status);
        public Task UpdateTicketShemePlaneAsync(int wayId, int planeId);
        public Task DeleteTicketsAsync(DateTime departureDate, DateTime arrivalDate, int ticketScheme);
    }
}
