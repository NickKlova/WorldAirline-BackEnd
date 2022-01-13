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
    public class WayManagment : Interfaces.IWay
    {
        private WorldAirlinesClient _db;
        public WayManagment(WorldAirlinesClient dbClient)
        {
            _db = dbClient;
        }
        public async Task<IEnumerable<ReturnWay>> GetAllWaysAsync()
        {
            await using (_db)
            {
                var ways = _db.context.Ways
                    .Include(x => x.ArrivalAirport)
                    .Include(x => x.ArrivalAirport.Location)
                    .Include(x => x.DepartureAirport)
                    .Include(x => x.DepartureAirport.Location)
                    .ToListAsync()
                    .Result;

                if (ways == null)
                    return null;

                List<ReturnWay> response = new List<ReturnWay>();

                foreach (var way in ways)
                {
                    ReturnWay item = new ReturnWay
                    {
                        Id = way.Id,
                        FlightDuration = way.FlightDuration.TotalMinutes.ToString() + "minutes",
                        DepartureAirport = new ReturnAirport
                        {
                            Id = way.DepartureAirport.Id,
                            Name = way.DepartureAirport.Name,
                            Location = new ReturnLocation
                            {
                                Id = way.DepartureAirport.Location.Id,
                                City = way.DepartureAirport.Location.City,
                                Country = way.DepartureAirport.Location.Country
                            }
                        },
                        ArrivalAirport = new ReturnAirport
                        {
                            Id = way.ArrivalAirport.Id,
                            Name = way.ArrivalAirport.Name,
                            Location = new ReturnLocation
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
        public async Task<ReturnWay> GetWayAsync(int? id)
        {
            await using (_db)
            {
                var way = _db.context.Ways
                    .Include(x => x.ArrivalAirport)
                    .Include(x => x.ArrivalAirport.Location)
                    .Include(x => x.DepartureAirport)
                    .Include(x => x.DepartureAirport.Location)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (way == null)
                    return null;

                ReturnWay response = new ReturnWay
                {
                    Id = way.Id,
                    FlightDuration = way.FlightDuration.TotalMinutes.ToString() + " minutes",
                    DepartureAirport = new ReturnAirport
                    {
                        Id = way.DepartureAirport.Id,
                        Name = way.DepartureAirport.Name,
                        Location = new ReturnLocation
                        {
                            Id = way.DepartureAirport.Location.Id,
                            City = way.DepartureAirport.Location.City,
                            Country = way.DepartureAirport.Location.Country
                        }
                    },
                    ArrivalAirport = new ReturnAirport
                    {
                        Id = way.ArrivalAirport.Id,
                        Name = way.ArrivalAirport.Name,
                        Location = new ReturnLocation
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
        public async Task CreateWayAsync(ReceivedWay incomingData)
        {
            await using (_db)
            {
                Models.DB_Request.Way item = new Models.DB_Request.Way
                {
                    FlightDuration = incomingData.FlightDuration,
                    ArrivalAirportId = incomingData.ArrivalAirportId,
                    DepartureAirportId = incomingData.DepartureAirportId,
                    Actual = incomingData.Actual
                };

                _db.context.Add(item);
                _db.context.SaveChanges();
            }
        }
        public async Task ChangeWayActualityAsync(int wayId, bool actuality)
        {
            await using (_db)
            {
                var way = _db.context.Ways
                    .Include(x => x.ArrivalAirport)
                    .Include(x => x.ArrivalAirport.Location)
                    .Include(x => x.DepartureAirport)
                    .Include(x => x.DepartureAirport.Location)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == wayId);

                if (way == null)
                    throw new Exception("The specified path was not found in the database!");

                way.Actual = actuality;

                _db.context.SaveChanges();
            }
        }
        public async Task DeleteWayAsync(int wayId)
        {
            await using (_db)
            {
                var way = _db.context.Ways
                    .Include(x => x.ArrivalAirport)
                    .Include(x => x.ArrivalAirport.Location)
                    .Include(x => x.DepartureAirport)
                    .Include(x => x.DepartureAirport.Location)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == wayId);

                if (way == null)
                    throw new Exception("The specified path was not found in the database!");

                _db.context.Remove(way);
                _db.context.SaveChanges();
            }
        }
    }
}
