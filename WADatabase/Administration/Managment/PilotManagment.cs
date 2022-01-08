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
    public class PilotManagment
    {
        public async Task<Models.API.Response.ReturnPilot> GetPilotByLogin(string login)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var pilots = db.context.Pilots
                    .Include(x => x.Account)
                    .Include(x=>x.Account.Role)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Account.Login == login);

                Models.API.Response.ReturnPilot response = new Models.API.Response.ReturnPilot
                {
                    Id = pilots.Id,
                    Account = new Models.API.Response.ReturnAccount
                    {
                        Id = pilots.Account.Id,
                        Name = pilots.Account.Name,
                        Surname = pilots.Account.Surname,
                        Email = pilots.Account.Email,
                        Phone = pilots.Account.Phone,
                        Balance = pilots.Account.Balance,
                        Login = pilots.Account.Login,
                        Role = new Models.API.Response.ReturnRole
                        {
                            Id = pilots.Account.Role.Id,
                            Role = pilots.Account.Role.Role1
                        }
                    }
                };

                return response;
            }
        }

        public async Task<IEnumerable<Models.API.Response.ReturnPilot>> GetPilotByCredentials(string name, string surname)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var pilots = db.context.Pilots
                    .Include(x => x.Account)
                    .Include(x => x.Account.Role)
                    .ToListAsync()
                    .Result
                    .Where(x => x.Account.Name == name && x.Account.Surname == surname);

                List<Models.API.Response.ReturnPilot> response = new List<Models.API.Response.ReturnPilot>();

                foreach(var pilot in pilots)
                {
                    Models.API.Response.ReturnPilot item = new Models.API.Response.ReturnPilot
                    {
                        Id = pilot.Id,
                        Account = new Models.API.Response.ReturnAccount
                        {
                            Id = pilot.Account.Id,
                            Name = pilot.Account.Name,
                            Surname = pilot.Account.Surname,
                            Email = pilot.Account.Email,
                            Phone = pilot.Account.Phone,
                            Balance = pilot.Account.Balance,
                            Login = pilot.Account.Login,
                            Role = new Models.API.Response.ReturnRole
                            {
                                Id = pilot.Account.Role.Id,
                                Role = pilot.Account.Role.Role1
                            }
                        }
                    };
                    response.Add(item);
                }

                return response;
            }
        }
    
        public async Task CreatePilot(ReceivedPilot incomingData)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                Models.DB_Request.Pilot pilot = new Models.DB_Request.Pilot
                {
                    FlyingHours = incomingData.FlyingHours,
                    AccountId = incomingData.AccountId
                };

                db.context.Add(pilot);
                db.context.SaveChanges();
            }
        }

        public async Task UpdateFlyingHours(int amount, string login)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var pilot = db.context.Pilots
                                    .Include(x => x.Account)
                                    .ToListAsync()
                                    .Result
                                    .FirstOrDefault(x => x.Account.Login == login);

                pilot.FlyingHours += amount;
                db.context.Update(pilot);
                db.context.SaveChanges();
            }
        }

        public async Task DeletePilotByLogin(string login)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var pilot = db.context.Pilots
                    .Include(x => x.Account)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Account.Login == login);

                db.context.Remove(pilot);
                db.context.SaveChanges();
            }
        }
    }
}
