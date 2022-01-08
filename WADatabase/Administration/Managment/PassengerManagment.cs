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
    public class PassengerManagment
    {
        public async Task CreatePassengerAsync(ReceivedPassenger incomingData)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            Models.DB_Request.Passenger passenger = new Models.DB_Request.Passenger
            {
                Name = incomingData.Name,
                Surname = incomingData.Surname,
                Email = incomingData.Email,
                PassportSeries = incomingData.PassportSeries
            };

            await using (db.context)
            {
                var result = db.context.Add(passenger);

                if (result.State != EntityState.Added)
                    throw new Exception("Bad request");

                db.context.SaveChanges();
            }
        }
        public async Task<Models.API.Response.ReturnPassenger> GetPassengerByPassportSeriesAsync(string passportSeries)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();
            
            await using (db.context)
            {
                var passenger = db.context.Passengers
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.PassportSeries == passportSeries);

                Models.API.Response.ReturnPassenger response = new Models.API.Response.ReturnPassenger
                {
                    Id  = passenger.Id,
                    Name = passenger.Name,
                    Surname = passenger.Surname,
                    PassportSeries = passenger.PassportSeries,
                    Email = passenger.Email
                };

                return response;
            }
        }

        public async Task<IEnumerable<Models.API.Response.ReturnPassenger>> GetPassengerBySurname(string surname)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var passengers = db.context.Passengers
                    .ToListAsync()
                    .Result
                    .Where(x => x.Surname == surname);

                List<Models.API.Response.ReturnPassenger> response = new List<Models.API.Response.ReturnPassenger>();

                foreach(var passenger in passengers)
                {
                    Models.API.Response.ReturnPassenger item = new Models.API.Response.ReturnPassenger
                    {
                        Id = passenger.Id,
                        Name = passenger.Name,
                        Surname = passenger.Surname,
                        PassportSeries = passenger.PassportSeries,
                        Email = passenger.Email
                    };

                    response.Add(item);
                }

                return response;
            }
        }

        public async Task DeletePassengerByPassportSeries(string passportSeries)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var passengers = db.context.Passengers
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.PassportSeries == passportSeries);

                db.context.Remove(passengers);

                db.context.SaveChanges();
            }
        }
    }
}
