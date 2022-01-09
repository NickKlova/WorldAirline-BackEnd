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
    public class AirportManagment : Interfaces.IAirport
    {
        private WorldAirlinesClient _db;
        public AirportManagment(WorldAirlinesClient dbClient)
        {
            _db = dbClient;
        }
        public async Task<ReturnAirport> GetAirportAsync(int id)
        {
            await using (_db)
            {
                var airport = _db.context.Airports
                    .Include(x => x.Location)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                ReturnAirport response = new ReturnAirport
                {
                    Id = airport.Id,
                    Location = new ReturnLocation
                    {
                        Id = airport.Location.Id,
                        City = airport.Location.City,
                        Country = airport.Location.Country
                    }
                };

                return response;
            }
        }
        public async Task<IEnumerable<ReturnAirport>> GetAirportsAsync(string country)
        {
            await using (_db)
            {
                var airports = _db.context.Airports
                    .Include(x => x.Location)
                    .ToListAsync()
                    .Result
                    .Where(x => x.Location.Country == country);

                if (airports == null)
                    return null;

                List<ReturnAirport> response = new List<ReturnAirport>();

                foreach (var airport in airports)
                {
                    ReturnAirport item = new ReturnAirport
                    {
                        Id = airport.Id,
                        Location = new Models.API.Response.ReturnLocation
                        {
                            Id = airport.Location.Id,
                            City = airport.Location.City,
                            Country = airport.Location.Country
                        }
                    };

                    response.Add(item);
                }
                return response;
            }
        }
        public async Task<IEnumerable<ReturnAirport>> GetAllAirportsAsync()
        {
            await using (_db)
            {
                var airports = _db.context.Airports
                    .Include(x => x.Location)
                    .ToListAsync()
                    .Result;

                if (airports == null)
                    return null;

                List<ReturnAirport> response = new List<ReturnAirport>();

                foreach (var airport in airports)
                {
                    ReturnAirport item = new ReturnAirport
                    {
                        Id = airport.Id,
                        Location = new ReturnLocation
                        {
                            Id = airport.Location.Id,
                            City = airport.Location.City,
                            Country = airport.Location.Country
                        }
                    };

                    response.Add(item);
                }
                return response;
            }
        }
        public async Task CreateAirportAsync(ReceivedAirport incomingData)
        {
            await using (_db)
            {
                var loc = _db.context.Locations
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.City == incomingData.Location.City && x.Country == incomingData.Location.Country);

                Models.DB_Request.Airport item = new Models.DB_Request.Airport
                {
                    Name = incomingData.Name,
                    LocationId = loc.Id
                };

                _db.context.Add(item);
                _db.context.SaveChanges();
            }
        }
        public async Task DeleteAirportAsync(int id)
        {
            await using (_db)
            {
                var airport = _db.context.Airports
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (airport == null)
                    throw new Exception("Bad data!");

                _db.context.Remove(airport);
                _db.context.SaveChanges();
            }
        }
    }
}
