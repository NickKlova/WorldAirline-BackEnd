using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;
using WADatabase.Models.API.Request;

namespace WADatabase.Administration.Managment
{
    public class TicketManagment
    {
        public async Task<IEnumerable<Models.API.Response.ReturnTicketScheme>> GetTicketSchemeByWayId(int wayId)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            List<Models.API.Response.ReturnTicketScheme> response = new List<Models.API.Response.ReturnTicketScheme>();

            Models.API.Response.ReturnTicketScheme ticketShemeItem;

            await using (db.context)
            {
                var tickets = db.context.TicketSchemes
                    .Include(x=>x.Plane)
                    .Include(x=>x.Way)
                    .Include(x=>x.Way.DepartureAirport)
                    .Include(x => x.Way.DepartureAirport.Location)
                    .Include(x => x.Way.ArrivalAirport)
                    .Include(x => x.Way.ArrivalAirport.Location)
                    .ToListAsync()
                    .Result
                    .Where(x => x.WayId == wayId);

                foreach(var ticket in tickets)
                {
                    Models.API.Response.ReturnPlane plane;
                    if (ticket.PlaneId == null)
                    {
                        plane = null;
                    }
                    else
                    {
                        plane = new Models.API.Response.ReturnPlane
                        {
                            Id = ticket.Plane.Id,
                            Model = ticket.Plane.Model,
                            Number = ticket.Plane.Number,
                            ManufactureDate = ticket.Plane.ManufactureDate,
                            LifeTime = ticket.Plane.LifeTime,
                            Ok = ticket.Plane.Ok
                        };
                        
                    }

                    ticketShemeItem = new Models.API.Response.ReturnTicketScheme
                    {
                        Way = new Models.API.Response.ReturnWay
                        {
                            Id = ticket.Way.Id,
                            FlightDuration = ticket.Way.FlightDuration,
                            DepartureAirport = new Models.API.Response.ReturnAirport
                            {
                                Id = ticket.Way.DepartureAirport.Id,
                                Name = ticket.Way.DepartureAirport.Name,
                                Location = new Models.API.Response.ReturnLocation
                                {
                                    Id = ticket.Way.DepartureAirport.Location.Id,
                                    City = ticket.Way.DepartureAirport.Location.City,
                                    Country = ticket.Way.DepartureAirport.Location.Country
                                }
                            },
                            ArrivalAirport = new Models.API.Response.ReturnAirport
                            {
                                Id = ticket.Way.ArrivalAirport.Id,
                                Name = ticket.Way.ArrivalAirport.Name,
                                Location = new Models.API.Response.ReturnLocation
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
            }

            return response;
        }
        public async Task<IEnumerable<Models.API.Response.ReturnTicketScheme>> GetTicketSchemeById(int id)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            List<Models.API.Response.ReturnTicketScheme> response = new List<Models.API.Response.ReturnTicketScheme>();

            Models.API.Response.ReturnTicketScheme ticketShemeItem;

            await using (db.context)
            {
                var tickets = db.context.TicketSchemes
                    .Include(x => x.Plane)
                    .Include(x => x.Way)
                    .Include(x => x.Way.DepartureAirport)
                    .Include(x => x.Way.DepartureAirport.Location)
                    .Include(x => x.Way.ArrivalAirport)
                    .Include(x => x.Way.ArrivalAirport.Location)
                    .ToListAsync()
                    .Result
                    .Where(x => x.Id == id);

                foreach (var ticket in tickets)
                {
                    Models.API.Response.ReturnPlane plane;
                    if (ticket.PlaneId == null)
                    {
                        plane = null;
                    }
                    else
                    {
                        plane = new Models.API.Response.ReturnPlane
                        {
                            Id = ticket.Plane.Id,
                            Model = ticket.Plane.Model,
                            Number = ticket.Plane.Number,
                            ManufactureDate = ticket.Plane.ManufactureDate,
                            LifeTime = ticket.Plane.LifeTime,
                            Ok = ticket.Plane.Ok
                        };

                    }

                    ticketShemeItem = new Models.API.Response.ReturnTicketScheme
                    {
                        Way = new Models.API.Response.ReturnWay
                        {
                            Id = ticket.Way.Id,
                            FlightDuration = ticket.Way.FlightDuration,
                            DepartureAirport = new Models.API.Response.ReturnAirport
                            {
                                Id = ticket.Way.DepartureAirport.Id,
                                Name = ticket.Way.DepartureAirport.Name,
                                Location = new Models.API.Response.ReturnLocation
                                {
                                    Id = ticket.Way.DepartureAirport.Location.Id,
                                    City = ticket.Way.DepartureAirport.Location.City,
                                    Country = ticket.Way.DepartureAirport.Location.Country
                                }
                            },
                            ArrivalAirport = new Models.API.Response.ReturnAirport
                            {
                                Id = ticket.Way.ArrivalAirport.Id,
                                Name = ticket.Way.ArrivalAirport.Name,
                                Location = new Models.API.Response.ReturnLocation
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
            }

            return response;
        }
        public async Task CreateTicketSchemeAsync(ReceiveTicketScheme incomingTicketScheme)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            Models.DB_Request.TicketScheme ticketScheme = new Models.DB_Request.TicketScheme
            {
                WayId = incomingTicketScheme.WayId,
                PlaneId = incomingTicketScheme.PlaneId,
                DepartureDate = incomingTicketScheme.DepartureDate,
                ArrivalDate = incomingTicketScheme.ArrivalDate,
                Canceled = incomingTicketScheme.Canceled
            };

            await using (db.context)
            {
                var result = db.context.Add(ticketScheme);

                if (result.State != EntityState.Added)
                    throw new Exception("Bad request");

                db.context.SaveChanges();
            }
        }
        public async Task FlightStatusChangeByIdAsync(int id, bool status)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var ticketSchemes = db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                ticketSchemes.Canceled = status;
                db.context.Update(ticketSchemes);
                db.context.SaveChanges();
            }
        }
        public async Task FlightStatusChangeByWayAsync(int wayId, DateTime departureDate, bool status)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var ticketSchemes = db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .Where(x => x.WayId == wayId && x.DepartureDate == departureDate);

                foreach(var ticket in ticketSchemes)
                {
                    ticket.Canceled = status;
                    db.context.Update(ticket);
                }

                db.context.SaveChanges();
            }
        }
        public async Task UpdateTicketShemePlaneByWayIdAsync(int wayId, int planeId)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var ticketSchemes = db.context.TicketSchemes
                    .ToListAsync()
                    .Result
                    .Where(x => x.WayId == wayId);
                if (ticketSchemes == null)
                    throw new Exception("Not found");

                var planes = db.context.Planes
                    .ToListAsync()
                    .Result
                    .Where(x => x.Id == planeId);
                if (planes == null)
                    throw new Exception("Not found");

                foreach(var ticket in ticketSchemes)
                {
                    db.context.Update(ticket);
                }

                db.context.SaveChanges();
            }
        }
        public async Task CreateTicketsAsync(int ticketAmount, string travelClass, decimal price, ReceivedTicket incomingTicket)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var classes = db.context.TravelClasses
                    .ToListAsync()
                    .Result.FirstOrDefault(x => x.ClassName == travelClass);

                if (classes == null)
                    throw new Exception("Class not found");

                for(int i= 0; i < ticketAmount; i++)
                {
                    Models.DB_Request.Ticket ticket = new Models.DB_Request.Ticket
                    {
                        TicketSchemeId = incomingTicket.TicketSchemeId,
                        Seat = i+1,
                        TravelClassId = classes.Id,
                        Price = price,
                        Booked = false
                    };

                    var result = db.context.Add(ticket);

                    if (result.State != EntityState.Added)
                        throw new Exception("Bad request");
                }

                db.context.SaveChanges();
            }
            }
        public async Task DeleteTicketsAsync(DateTime departureDate, DateTime arrivalDate, int ticketScheme)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var tickets = db.context.Tickets
                    .Include(x=>x.TicketScheme)
                    .ToListAsync()
                    .Result
                    .Where(x => x.TicketSchemeId == ticketScheme && x.TicketScheme.DepartureDate == departureDate && x.TicketScheme.ArrivalDate == arrivalDate);

                foreach(var ticket in tickets)
                {
                    db.context.Remove(ticket);
                }

                db.context.SaveChanges();
            }
        }
    }
}
