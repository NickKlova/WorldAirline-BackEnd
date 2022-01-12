using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment.Interfaces
{
    public interface ITicket
    {
        public Task<IEnumerable<Models.API.Response.ReturnTicketScheme>> GetTicketSchemeByWayIdAsync(int? wayId);
        public Task<Models.API.Response.ReturnTicketScheme> GetTicketSchemeByIdAsync(int? id);
        public Task<IEnumerable<Models.API.Response.ReturnTicket.TicketFullInfo>> GetFullInfoTicketsAsync(int ticketSchemeId);
        public Task<IEnumerable<Models.API.Response.ReturnTicket.TicketShortInfo>> GetShortInfoTicketsAsync(int ticketSchemeId);
        public Task<IEnumerable<Models.API.Response.ReturnTicket.TicketFullInfo>> GetFullInfoBookedTicketAsync(int ticketSchemeId);
        public Task<IEnumerable<Models.API.Response.ReturnTicket.TicketFullInfo>> GetFullInfoUnBookedTicketAsync(int ticketSchemeId);
        public Task<IEnumerable<Models.API.Response.ReturnTicket.TicketShortInfo>> GetShortInfoBookedTicketAsync(int ticketSchemeId);
        public Task<IEnumerable<Models.API.Response.ReturnTicket.TicketShortInfo>> GetShortInfoUnBookedTicketAsync(int ticketSchemeId);
        public Task CreateTicketSchemeAsync(Models.API.Request.ReceiveTicketScheme incomingTicketScheme);
        public Task CreateTicketsAsync(Models.API.Request.ReceivedTicket incomingTicket);
        public Task<Models.API.Response.ReturnBuyTicket> BuyTicketAsync(string login, Models.API.Request.ReceivedPassenger passenger, Models.API.Request.ReceivedBuyTicket info);
        public Task FlightStatusChangeByIdAsync(int id, bool status);
        public Task FlightStatusChangeBySchemeIdAsync(int schemeId, bool status);
        public Task UpdateTicketShemePlaneAsync(int schemeId, int planeId);
        public Task UpdateTicketPriceAsync(int ticketSchemeId, decimal price);
        public Task DeleteTicketSchemeAsync(int ticketSchemeId);
        public Task DeleteTicketsAsync(int ticketScheme);
    }
}
