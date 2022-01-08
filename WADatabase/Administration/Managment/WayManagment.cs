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
    public class WayManagment
    {
        public async Task<IEnumerable<Models.API.Response.ReturnWay>> GetAllWays()
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var ways = db.context.Ways
                    .Include(x=>x.ArrivalAirport)
                    .Include(x=>x.ArrivalAirport.Location)
                    .Include(x=>x.DepartureAirport)
                    .Include(x=>x.DepartureAirport.Location)
                    .ToListAsync()
                    .Result;

                List<Models.API.Response.ReturnWay> response = new List<Models.API.Response.ReturnWay>(); 
                foreach(var way in ways)
                {
                    Models.API.Response.ReturnWay item = new Models.API.Response.ReturnWay
                    {
                        Id = way.Id,
                        FlightDuration = way.FlightDuration,
                        DepartureAirport = new Models.API.Response.ReturnAirport
                        {
                            Id = way.DepartureAirport.Id,
                            Name = way.DepartureAirport.Name,
                            Location = new Models.API.Response.ReturnLocation
                            {
                                Id = way.DepartureAirport.Location.Id,
                                City = way.DepartureAirport.Location.City,
                                Country = way.DepartureAirport.Location.Country
                            }
                        },
                        ArrivalAirport = new Models.API.Response.ReturnAirport
                        {
                            Id = way.ArrivalAirport.Id,
                            Name = way.ArrivalAirport.Name,
                            Location = new Models.API.Response.ReturnLocation
                            {
                                Id = way.ArrivalAirport.Location.Id,
                                City = way.ArrivalAirport.Location.City,
                                Country = way.ArrivalAirport.Location.Country
                            }
                        },
                        Actual = way.Actual
                    };

                    response.Add(item);
                }

                return response;
            }
        }

        public async Task<Models.API.Response.ReturnWay> GetWayById(int id)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var way = db.context.Ways
                    .Include(x => x.ArrivalAirport)
                    .Include(x => x.ArrivalAirport.Location)
                    .Include(x => x.DepartureAirport)
                    .Include(x => x.DepartureAirport.Location)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x=>x.Id == id);

                    Models.API.Response.ReturnWay response = new Models.API.Response.ReturnWay
                    {
                        Id = way.Id,
                        FlightDuration = way.FlightDuration,
                        DepartureAirport = new Models.API.Response.ReturnAirport
                        {
                            Id = way.DepartureAirport.Id,
                            Name = way.DepartureAirport.Name,
                            Location = new Models.API.Response.ReturnLocation
                            {
                                Id = way.DepartureAirport.Location.Id,
                                City = way.DepartureAirport.Location.City,
                                Country = way.DepartureAirport.Location.Country
                            }
                        },
                        ArrivalAirport = new Models.API.Response.ReturnAirport
                        {
                            Id = way.ArrivalAirport.Id,
                            Name = way.ArrivalAirport.Name,
                            Location = new Models.API.Response.ReturnLocation
                            {
                                Id = way.ArrivalAirport.Location.Id,
                                City = way.ArrivalAirport.Location.City,
                                Country = way.ArrivalAirport.Location.Country
                            }
                        },
                        Actual = way.Actual
                    };

                return response;
            }
        }

        public async Task CreateWay(ReceivedWay incomingData) 
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                Models.DB_Request.Way item = new Models.DB_Request.Way
                {
                    FlightDuration = incomingData.FlightDuration,
                    ArrivalAirportId = incomingData.ArrivalAirportId,
                    DepartureAirportId = incomingData.DepartureAirportId,
                    Actual = incomingData.Actual
                };

                db.context.Add(item);
                db.context.SaveChanges();
            }
        }

        public async Task ChangeWayActuality(int wayId, bool actuality)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var ways = db.context.Ways
                    .Include(x => x.ArrivalAirport)
                    .Include(x => x.ArrivalAirport.Location)
                    .Include(x => x.DepartureAirport)
                    .Include(x => x.DepartureAirport.Location)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x=>x.Id == wayId);

                ways.Actual = actuality;

                db.context.SaveChanges();
            }
        }

        public async Task DeleteWay(int wayId)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var ways = db.context.Ways
                    .Include(x => x.ArrivalAirport)
                    .Include(x => x.ArrivalAirport.Location)
                    .Include(x => x.DepartureAirport)
                    .Include(x => x.DepartureAirport.Location)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == wayId);

                db.context.Remove(ways);
                db.context.SaveChanges();
            }
        }
    }
}
