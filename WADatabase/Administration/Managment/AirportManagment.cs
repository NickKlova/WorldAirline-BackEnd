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
    public class AirportManagment
    {
        public async Task CreateAirport(ReceivedAirport incomingData)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var loc = db.context.Locations
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.City == incomingData.Location.City && x.Country == incomingData.Location.Country);

                Models.DB_Request.Airport item = new Models.DB_Request.Airport
                {
                    Name = incomingData.Name,
                    LocationId = loc.Id
                };

                db.context.Add(item);
                db.context.SaveChanges();
            }
        }

        public async Task DeleteAirport(int id)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var airport = db.context.Airports
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                db.context.Remove(airport);
                db.context.SaveChanges();
            }
        }

        public async Task<IEnumerable<Models.API.Response.ReturnAirport>> GetAirportsByCountry(string country)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var airports = db.context.Airports
                    .Include(x=>x.Location)
                    .ToListAsync()
                    .Result
                    .Where(x => x.Location.Country == country);

                List<Models.API.Response.ReturnAirport> response = new List<Models.API.Response.ReturnAirport>();

                foreach(var airport in airports)
                {
                    Models.API.Response.ReturnAirport item = new Models.API.Response.ReturnAirport
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

        public async Task<Models.API.Response.ReturnAirport> GetAirport(int id)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var airport = db.context.Airports
                    .Include(x => x.Location)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);


                    Models.API.Response.ReturnAirport item = new Models.API.Response.ReturnAirport
                    {
                        Id = airport.Id,
                        Location = new Models.API.Response.ReturnLocation
                        {
                            Id = airport.Location.Id,
                            City = airport.Location.City,
                            Country = airport.Location.Country
                        }
                    };

                
                return item;
            }
        }

        public async Task<IEnumerable<Models.API.Response.ReturnAirport>> GetAllAirports()
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var airports = db.context.Airports
                    .Include(x => x.Location)
                    .ToListAsync()
                    .Result;

                List<Models.API.Response.ReturnAirport> response = new List<Models.API.Response.ReturnAirport>();

                foreach (var airport in airports)
                {
                    Models.API.Response.ReturnAirport item = new Models.API.Response.ReturnAirport
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
    }
}
