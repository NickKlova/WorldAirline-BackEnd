using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;
using WADatabase.Models.API.Request;
using WADatabase.Models.API.Response;
using WADatabase.Models.API.Response.ReturnTicket;

namespace WADatabase.Administration.Managment
{
    public class TicketManagment : Interfaces.ITicket
    {
        private WorldAirlinesClient _db;
        private AccountManagment _accountDb;
        private PassengerManagment _passengerDb;
        private PlaneManagment _planeDb;
        private WayManagment _wayDb;
        public TicketManagment(WorldAirlinesClient dbClient, AccountManagment accountClient, PassengerManagment passengerClient, PlaneManagment planeClient, WayManagment wayClient)
        {
            _db = dbClient;
            _accountDb = accountClient;
            _passengerDb = passengerClient;
            _planeDb = planeClient;
            _wayDb = wayClient;
        }
        public async Task<IEnumerable<ReturnTicketScheme>> GetTicketSchemeByWayIdAsync(int? wayId)
        {
            if (wayId == null)
                return null;

            await using (_db)
            {
                List<ReturnTicketScheme> response = new List<ReturnTicketScheme>();

                ReturnTicketScheme ticketShemeItem;

                var tickets = _db.context.TicketSchemes
                    .Include(x => x.Plane)
                    .Include(x => x.Way)
                    .Include(x => x.Way.DepartureAirport)
                    .Include(x => x.Way.DepartureAirport.Location)
                    .Include(x => x.Way.ArrivalAirport)
                    .Include(x => x.Way.ArrivalAirport.Location)
                    .ToListAsync()
                    .Result
                    .Where(x => x.WayId == wayId);

                if (tickets.Count() == 0)
                    return null;

                foreach (var ticket in tickets)
                {
                    ticketShemeItem = new ReturnTicketScheme
                    {
                        Way = await _wayDb.GetWayAsync(ticket.WayId),
                        Plane = await _planeDb.GetPlaneAsync(ticket.PlaneId),
                        DepartureDate = ticket.DepartureDate,
                        ArrivalDate = ticket.ArrivalDate,
                        Canceled = ticket.Canceled
                    };

                    response.Add(ticketShemeItem);
                }
                return response;
            }
        }
        public async Task<ReturnTicketScheme> GetTicketSchemeByIdAsync(int? id)
        {
            if (id == null)
                return null;

            await using (_db)
            {
                var ticket = _db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (ticket == null)
                    return null;

                ReturnTicketScheme response = new ReturnTicketScheme
                {
                    Way = await _wayDb.GetWayAsync(ticket.WayId),
                    Plane = await _planeDb.GetPlaneAsync(ticket.PlaneId),
                    DepartureDate = ticket.DepartureDate,
                    ArrivalDate = ticket.ArrivalDate,
                    Canceled = ticket.Canceled
                };
                return response;
            }
        }
        public async Task<IEnumerable<TicketFullInfo>> GetFullInfoTicketsAsync(int ticketSchemeId)
        {
            await using (_db)
            {
                var tickets = _db.context.Tickets
                    .Include(x => x.TicketScheme)
                    .Include(x => x.TravelClass)
                    .ToListAsync()
                    .Result
                    .Where(x => x.TicketSchemeId == ticketSchemeId);

                if (tickets.Count() == 0)
                    return null;

                List<TicketFullInfo> response = new List<TicketFullInfo>();

                foreach (var ticket in tickets)
                {
                    TicketFullInfo item = new TicketFullInfo
                    {
                        Id = ticket.Id,
                        Code = ticket.Code,
                        TicketScheme = await GetTicketSchemeByIdAsync(ticket.TicketSchemeId),
                        Seat = ticket.Seat,
                        TravelClass = new ReturnTravelClass
                        {
                            Id = ticket.TravelClass.Id,
                            TravelClass = ticket.TravelClass.ClassName
                        },
                        BaggageWeight = ticket.BaggageWeight,
                        Price = ticket.Price,
                        Booked = ticket.Booked,
                        Account = await _accountDb.GetAccountAsync(ticket.AccountId),
                        Passenger = await _passengerDb.GetPassengerAsync(ticket.PassengerId)
                    };

                    response.Add(item);
                }

                return response;
            }
        }
        public async Task<IEnumerable<TicketShortInfo>> GetShortInfoTicketsAsync(int ticketSchemeId)
        {
            await using (_db)
            {
                var tickets = _db.context.Tickets
                    .Include(x => x.TicketScheme)
                    .Include(x => x.TicketScheme.Way.DepartureAirport)
                    .Include(x => x.TicketScheme.Way.ArrivalAirport)
                    .Include(x => x.TravelClass)
                    .ToListAsync()
                    .Result
                    .Where(x => x.TicketSchemeId == ticketSchemeId);

                if (tickets.Count() == 0)
                    return null;

                List<TicketShortInfo> response = new List<TicketShortInfo>();

                foreach (var ticket in tickets)
                {
                    TicketShortInfo item = new TicketShortInfo
                    {
                        Id = ticket.Id,
                        Code = ticket.Code,
                        DepartureDate = ticket.TicketScheme.DepartureDate.ToString(),
                        ArrivalDate = ticket.TicketScheme.ArrivalDate.ToString(),
                        DepartureAirport = ticket.TicketScheme.Way.DepartureAirport.Name,
                        ArrivalAirport = ticket.TicketScheme.Way.ArrivalAirport.Name,
                        Seat = ticket.Seat,
                        TravelClass = ticket.TravelClass.ClassName,
                        Booked = ticket.Booked,
                        Price = ticket.Price,
                        Canceled = ticket.TicketScheme.Canceled
                    };

                    response.Add(item);
                }

                return response;
            }
        }
        public async Task<IEnumerable<TicketFullInfo>> GetFullInfoBookedTicketAsync(int ticketSchemeId)
        {
            var response = await GetFullInfoTicketsAsync(ticketSchemeId);
            if (response == null)
                return null;
            else
                return response.Where(x => x.Booked == true);
        }
        public async Task<IEnumerable<TicketFullInfo>> GetFullInfoUnBookedTicketAsync(int ticketSchemeId)
        {
            var response = await GetFullInfoTicketsAsync(ticketSchemeId);
            if (response == null)
                return null;
            else
                return response.Where(x => x.Booked == false);
        }
        public async Task<IEnumerable<TicketShortInfo>> GetShortInfoBookedTicketAsync(int ticketSchemeId)
        {
            var response = await GetShortInfoTicketsAsync(ticketSchemeId);
            if (response == null)
                return null;
            else
                return response.Where(x => x.Booked == true);
        }
        public async Task<IEnumerable<TicketShortInfo>> GetShortInfoUnBookedTicketAsync(int ticketSchemeId)
        {
            var response = await GetShortInfoTicketsAsync(ticketSchemeId);
            if (response == null)
                return null;
            else
                return response.Where(x => x.Booked == false);
        }
        public async Task CreateTicketSchemeAsync(ReceiveTicketScheme incomingTicketScheme)
        {
            await using (_db)
            {
                Models.DB_Request.TicketScheme ticketScheme = new Models.DB_Request.TicketScheme
                {
                    WayId = incomingTicketScheme.WayId,
                    PlaneId = incomingTicketScheme.PlaneId,
                    DepartureDate = incomingTicketScheme.DepartureDate,
                    ArrivalDate = incomingTicketScheme.ArrivalDate,
                    Canceled = incomingTicketScheme.Canceled
                };

                _db.context.Add(ticketScheme);
                _db.context.SaveChanges();
            }
        }
        public async Task CreateTicketsAsync(ReceivedTicket incomingTicket)
        {
            await using (_db)
            {
                var classes = _db.context.TravelClasses
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.ClassName == incomingTicket.TravelClass);

                if (classes == null)
                    throw new Exception("Bad data!");

                for (int i = 0; i < incomingTicket.TicketAmount; i++)
                {
                    Models.DB_Request.Ticket ticket = new Models.DB_Request.Ticket
                    {
                        Code = Guid.NewGuid().ToString(),
                        TicketSchemeId = incomingTicket.TicketSchemeId,
                        Seat = incomingTicket.Seat + i,
                        TravelClassId = classes.Id,
                        Price = incomingTicket.Price,
                        Booked = false
                    };

                    _db.context.Add(ticket);
                }
                _db.context.SaveChanges();
            }
        }
        public async Task FlightStatusChangeByIdAsync(int id, bool status)
        {
            await using (_db)
            {
                var ticketSchemes = _db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (ticketSchemes == null)
                    throw new Exception("The specified record is not in the database!");

                ticketSchemes.Canceled = status;
                _db.context.SaveChanges();
            }
        }
        public async Task FlightStatusChangeBySchemeIdAsync(int schemeId, bool status)
        {
            await using (_db)
            {
                var ticketSchemes = _db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .Where(x => x.Id == schemeId);

                if (ticketSchemes == null)
                    throw new Exception("The specified schema is not in the database!");

                foreach (var ticket in ticketSchemes)
                {
                    ticket.Canceled = status;
                }

                _db.context.SaveChanges();
            }
        }
        public async Task UpdateTicketShemePlaneAsync(int schemeId, int planeId)
        {
            await using (_db)
            {
                var ticketSchemes = _db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .Where(x => x.Id == schemeId);

                if (ticketSchemes == null)
                    throw new Exception("The specified schema is not in the database!");

                var plane = _db.context.Planes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == planeId);

                if (plane == null)
                    throw new Exception("The specified plane is not in the database!");

                foreach (var ticket in ticketSchemes)
                {
                    ticket.PlaneId = plane.Id;
                }

                _db.context.SaveChanges();
            }
        }
        public async Task UpdateTicketPriceAsync(int ticketSchemeId, decimal price)
        {
            await using (_db)
            {
                var tickets = _db.context.Tickets
                    .ToListAsync()
                    .Result
                    .Where(x => x.TicketSchemeId == ticketSchemeId);

                if (tickets.Count() == 0)
                    throw new Exception("Bad data!");

                foreach (var ticket in tickets)
                {
                    ticket.Price = price;
                    _db.context.SaveChanges();
                }
            }
        }
        public async Task DeleteTicketSchemeAsync(int ticketSchemeId)
        {
            await using (_db)
            {
                var ticketScheme = _db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == ticketSchemeId);

                if (ticketScheme == null)
                    throw new Exception("There are no tickets for the given scheme in the database!");

                _db.context.Remove(ticketScheme);
                _db.context.SaveChanges();
            }
        }
        public async Task DeleteTicketsAsync(int ticketScheme)
        {
            await using (_db)
            {
                var tickets = _db.context.Tickets
                    .Include(x => x.TicketScheme)
                    .ToListAsync()
                    .Result
                    .Where(x => x.TicketSchemeId == ticketScheme);

                if (tickets.Count() == 0)
                    throw new Exception("There are no tickets for the given scheme in the database!");

                foreach (var ticket in tickets)
                {
                    _db.context.Remove(ticket);
                }

                _db.context.SaveChanges();
            }
        }
        public async Task DeleteTicketAsync(int ticketId)
        {
            await using (_db)
            {
                var ticket = _db.context.Tickets
                    .Include(x => x.TicketScheme)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == ticketId);

                if (ticket == null)
                    throw new Exception("There are no tickets for the given ID in the database!");

                _db.context.Remove(ticket);

                _db.context.SaveChanges();
            }
        }
    }
}
