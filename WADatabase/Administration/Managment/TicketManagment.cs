using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;
using WADatabase.Models.API.Request;
using WADatabase.Models.API.Response;

namespace WADatabase.Administration.Managment
{
    public class TicketManagment : Interfaces.ITicket
    {
        private WorldAirlinesClient _db;
        public TicketManagment(WorldAirlinesClient dbClient)
        {
            _db = dbClient;
        }
        public async Task<IEnumerable<ReturnTicketScheme>> GetTicketSchemeByWayIdAsync(int wayId)
        {
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

                if (tickets == null)
                    return null;

                foreach (var ticket in tickets)
                {
                    ReturnPlane plane;
                    if (ticket.PlaneId == null)
                    {
                        plane = null;
                    }
                    else
                    {
                        plane = new ReturnPlane
                        {
                            Id = ticket.Plane.Id,
                            Model = ticket.Plane.Model,
                            Number = ticket.Plane.Number,
                            ManufactureDate = ticket.Plane.ManufactureDate,
                            LifeTime = ticket.Plane.LifeTime,
                            Ok = ticket.Plane.Ok
                        };

                    }

                    ticketShemeItem = new ReturnTicketScheme
                    {
                        Way = new ReturnWay
                        {
                            Id = ticket.Way.Id,
                            FlightDuration = ticket.Way.FlightDuration,
                            DepartureAirport = new ReturnAirport
                            {
                                Id = ticket.Way.DepartureAirport.Id,
                                Name = ticket.Way.DepartureAirport.Name,
                                Location = new ReturnLocation
                                {
                                    Id = ticket.Way.DepartureAirport.Location.Id,
                                    City = ticket.Way.DepartureAirport.Location.City,
                                    Country = ticket.Way.DepartureAirport.Location.Country
                                }
                            },
                            ArrivalAirport = new ReturnAirport
                            {
                                Id = ticket.Way.ArrivalAirport.Id,
                                Name = ticket.Way.ArrivalAirport.Name,
                                Location = new ReturnLocation
                                {
                                    Id = ticket.Way.ArrivalAirport.Location.Id,
                                    City = ticket.Way.ArrivalAirport.Location.City,
                                    Country = ticket.Way.ArrivalAirport.Location.Country
                                }
                            },
                            Actual = ticket.Way.Actual
                        },
                        Plane = plane,
                        DepartureDate = ticket.DepartureDate,
                        ArrivalDate = ticket.ArrivalDate,
                        Canceled = ticket.Canceled
                    };

                    response.Add(ticketShemeItem);
                }
                return response;
            }
        }
        public async Task<IEnumerable<ReturnTicketScheme>> GetTicketSchemeByIdAsync(int id)
        {
            await using (_db)
            {
                var tickets = _db.context.TicketSchemes
                    .Include(x => x.Plane)
                    .Include(x => x.Way)
                    .Include(x => x.Way.DepartureAirport)
                    .Include(x => x.Way.DepartureAirport.Location)
                    .Include(x => x.Way.ArrivalAirport)
                    .Include(x => x.Way.ArrivalAirport.Location)
                    .ToListAsync()
                    .Result
                    .Where(x => x.Id == id);

                List<ReturnTicketScheme> response = new List<ReturnTicketScheme>();

                ReturnTicketScheme ticketShemeItem;

                foreach (var ticket in tickets)
                {
                    ReturnPlane plane;
                    if (ticket.PlaneId == null)
                    {
                        plane = null;
                    }
                    else
                    {
                        plane = new ReturnPlane
                        {
                            Id = ticket.Plane.Id,
                            Model = ticket.Plane.Model,
                            Number = ticket.Plane.Number,
                            ManufactureDate = ticket.Plane.ManufactureDate,
                            LifeTime = ticket.Plane.LifeTime,
                            Ok = ticket.Plane.Ok
                        };

                    }

                    ticketShemeItem = new ReturnTicketScheme
                    {
                        Way = new ReturnWay
                        {
                            Id = ticket.Way.Id,
                            FlightDuration = ticket.Way.FlightDuration,
                            DepartureAirport = new ReturnAirport
                            {
                                Id = ticket.Way.DepartureAirport.Id,
                                Name = ticket.Way.DepartureAirport.Name,
                                Location = new ReturnLocation
                                {
                                    Id = ticket.Way.DepartureAirport.Location.Id,
                                    City = ticket.Way.DepartureAirport.Location.City,
                                    Country = ticket.Way.DepartureAirport.Location.Country
                                }
                            },
                            ArrivalAirport = new ReturnAirport
                            {
                                Id = ticket.Way.ArrivalAirport.Id,
                                Name = ticket.Way.ArrivalAirport.Name,
                                Location = new ReturnLocation
                                {
                                    Id = ticket.Way.ArrivalAirport.Location.Id,
                                    City = ticket.Way.ArrivalAirport.Location.City,
                                    Country = ticket.Way.ArrivalAirport.Location.Country
                                }
                            },
                            Actual = ticket.Way.Actual
                        },
                        Plane = plane,
                        DepartureDate = ticket.DepartureDate,
                        ArrivalDate = ticket.ArrivalDate,
                        Canceled = ticket.Canceled
                    };

                    response.Add(ticketShemeItem);
                }
                return response;
            }
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
        public async Task CreateTicketsAsync(int ticketAmount, string travelClass, decimal price, ReceivedTicket incomingTicket)
        {
            await using (_db)
            {
                var classes = _db.context.TravelClasses
                    .ToListAsync()
                    .Result.FirstOrDefault(x => x.ClassName == travelClass);

                if (classes == null)
                    throw new Exception("Bad data!");

                for (int i = 0; i < ticketAmount; i++)
                {
                    Models.DB_Request.Ticket ticket = new Models.DB_Request.Ticket
                    {
                        TicketSchemeId = incomingTicket.TicketSchemeId,
                        Seat = i + 1,
                        TravelClassId = classes.Id,
                        Price = price,
                        Booked = false
                    };

                    _db.context.Add(ticket);
                }

                _db.context.SaveChanges();
            }
        }
        public async Task FlightStatusChangeAsync(int id, bool status)
        {
            await using (_db)
            {
                var ticketSchemes = _db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (ticketSchemes == null)
                    throw new Exception("Bad data!");

                ticketSchemes.Canceled = status;
                _db.context.SaveChanges();
            }
        }
        public async Task FlightStatusChangeAsync(int wayId, DateTime departureDate, bool status)
        {
            await using (_db)
            {
                var ticketSchemes = _db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .Where(x => x.WayId == wayId && x.DepartureDate == departureDate);

                if (ticketSchemes == null)
                    throw new Exception("Bad data!");

                foreach (var ticket in ticketSchemes)
                {
                    ticket.Canceled = status;
                }

                _db.context.SaveChanges();
            }
        }
        public async Task UpdateTicketShemePlaneAsync(int wayId, int planeId)
        {
            await using (_db)
            {
                var ticketSchemes = _db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .Where(x => x.WayId == wayId);

                if (ticketSchemes == null)
                    throw new Exception("Bad data!");

                var plane = _db.context.Planes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == planeId);

                if (plane == null)
                    throw new Exception("Bad data!");

                foreach (var ticket in ticketSchemes)
                {
                    ticket.PlaneId = plane.Id;
                }

                _db.context.SaveChanges();
            }
        }
        public async Task DeleteTicketsAsync(DateTime departureDate, DateTime arrivalDate, int ticketScheme)
        {
            await using (_db)
            {
                var tickets = _db.context.Tickets
                    .Include(x => x.TicketScheme)
                    .ToListAsync()
                    .Result
                    .Where(x => x.TicketSchemeId == ticketScheme && x.TicketScheme.DepartureDate == departureDate && x.TicketScheme.ArrivalDate == arrivalDate);

                foreach (var ticket in tickets)
                {
                    _db.context.Remove(ticket);
                }

                _db.context.SaveChanges();
            }
        }
    }
}
