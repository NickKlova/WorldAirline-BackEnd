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
    public class PassengerManagment /*: Interfaces.IPassenger*/
    {
        private WorldAirlinesClient _db;
        public PassengerManagment(WorldAirlinesClient dbClient)
        {
            _db = dbClient;
        }
        public async Task<IEnumerable<ReturnPassenger>> GetPassengersBySurnameAsync(string surname)
        {
            await using (_db)
            {
                var passengers = _db.context.Passengers
                    .ToListAsync()
                    .Result
                    .Where(x => x.Surname == surname);

                if (passengers.Count() == 0)
                    return null;

                List<ReturnPassenger> response = new List<ReturnPassenger>();

                foreach (var passenger in passengers)
                {
                    ReturnPassenger item = new ReturnPassenger
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

        public async Task<ReturnPassenger> GetPassengerAsync(int? id)
        {
            if (id == null)
                return null;

            await using (_db)
            {
                var passenger = _db.context.Passengers
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (passenger == null)
                    return null;

                ReturnPassenger response = new ReturnPassenger
                {
                    Id = passenger.Id,
                    Name = passenger.Name,
                    Surname = passenger.Surname,
                    PassportSeries = passenger.PassportSeries,
                    Email = passenger.Email
                };

                return response;
            }
        }

        public async Task CreatePassengerAsync(ReceivedPassenger incomingData)
        {
            if (!Settings.Validation.IsMailValid(incomingData.Email))
                throw new Exception("Wrong mail!");

            await using (_db)
            {
                Models.DB_Request.Passenger passenger = new Models.DB_Request.Passenger
                {
                    Name = incomingData.Name,
                    Surname = incomingData.Surname,
                    Email = incomingData.Email,
                    PassportSeries = incomingData.PassportSeries
                };

                _db.context.Add(passenger);
                _db.context.SaveChanges();
            }
        }

        public async Task DeletePassengerAsync(int id)
        {
            await using (_db)
            {
                var passenger = _db.context.Passengers
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (passenger == null)
                    throw new Exception("Bad data!");

                _db.context.Remove(passenger);
                _db.context.SaveChanges();
            }
        }
    }
}
