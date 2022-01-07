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
    }
}
