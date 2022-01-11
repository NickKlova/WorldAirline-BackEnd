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
    public class PilotManagment : Interfaces.IPilot
    {
        private WorldAirlinesClient _db;
        public PilotManagment(WorldAirlinesClient dbClient)
        {
            _db = dbClient;
        }
        public async Task<ReturnPilot> GetPilotAsync(string login)
        {
            await using (_db)
            {
                var pilots = _db.context.Pilots
                    .Include(x => x.Account)
                    .Include(x => x.Account.Role)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Account.Login == login);

                if (pilots == null)
                    return null;

                ReturnPilot response = new ReturnPilot
                {
                    Id = pilots.Id,
                    FlyingHours = pilots.FlyingHours,
                    Account = new ReturnAccount
                    {
                        Id = pilots.Account.Id,
                        Name = pilots.Account.Name,
                        Surname = pilots.Account.Surname,
                        Email = pilots.Account.Email,
                        Phone = pilots.Account.Phone,
                        Balance = pilots.Account.Balance,
                        Login = pilots.Account.Login,
                        Role = new ReturnRole
                        {
                            Id = pilots.Account.Role.Id,
                            Role = pilots.Account.Role.Role1
                        }
                    }
                };

                return response;
            }
        }
        public async Task<IEnumerable<ReturnPilot>> GetPilotByPersonalInfo(string name, string surname)
        {
            await using (_db)
            {
                var pilots = _db.context.Pilots
                    .Include(x => x.Account)
                    .Include(x => x.Account.Role)
                    .ToListAsync()
                    .Result
                    .Where(x => x.Account.Name == name && x.Account.Surname == surname);

                if (pilots.Count() == 0)
                    return null;

                List<ReturnPilot> response = new List<ReturnPilot>();

                foreach (var pilot in pilots)
                {
                    ReturnPilot item = new ReturnPilot
                    {
                        Id = pilot.Id,
                        FlyingHours = pilot.FlyingHours,
                        Account = new ReturnAccount
                        {
                            Id = pilot.Account.Id,
                            Name = pilot.Account.Name,
                            Surname = pilot.Account.Surname,
                            Email = pilot.Account.Email,
                            Phone = pilot.Account.Phone,
                            Balance = pilot.Account.Balance,
                            Login = pilot.Account.Login,
                            Role = new ReturnRole
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
        public async Task CreatePilotAsync(ReceivedPilot incomingData)
        {
            await using (_db)
            {
                var account = _db.context.Accounts
                    .Include(x => x.Role)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x=>x.Login == incomingData.Login);

                var role = _db.context.Roles
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Role1 == "pilot");

                if (account == null)
                    throw new Exception("Bad data!");


                Models.DB_Request.Pilot pilot = new Models.DB_Request.Pilot
                {
                    FlyingHours = incomingData.FlyingHours,
                    AccountId = account.Id
                };

                account.RoleId = role.Id;

                _db.context.Add(pilot);
                _db.context.SaveChanges();
            }
        }
        public async Task UpdateFlyingHoursAsync(int amount, string login)
        {
            await using (_db)
            {
                var pilot = _db.context.Pilots
                                    .Include(x => x.Account)
                                    .ToListAsync()
                                    .Result
                                    .FirstOrDefault(x => x.Account.Login == login);

                if (pilot == null)
                    throw new Exception("Bad data!");

                pilot.FlyingHours += amount;
                //_db.context.Update(pilot);
                _db.context.SaveChanges();
            }
        }
        public async Task DeletePilotAsync(int id)
        {
            await using (_db)
            {
                var pilot = _db.context.Pilots
                    .Include(x => x.Account)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (pilot == null)
                    throw new Exception("Bad data!");

                _db.context.Remove(pilot);
                _db.context.SaveChanges();
            }
        }
    }
}
