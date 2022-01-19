using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;
using WADatabase.Models.API.Response;

namespace WADatabase.Administration.Managment
{
    public class CrewManagment : Interfaces.ICrew
    {
        private readonly WorldAirlinesClient _db;
        public CrewManagment(WorldAirlinesClient dbClient)
        {
            _db = dbClient;
        }
        public async Task<IEnumerable<ReturnPilotCrew>> GetCrewByTicketSchemeAsync(int ticketId)
        {
            await using (_db)
            {
                List<ReturnPilotCrew> response = new List<ReturnPilotCrew>();

                var pilots = _db.context.Crews
                    .Include(x => x.CrewPosition)
                    .Include(x => x.Pilot)
                    .Include(x => x.Pilot.Account)
                    .Include(x => x.Pilot.Account.Role)
                    .ToListAsync()
                    .Result
                    .Where(x => x.TicketSchemeId == ticketId);

                if (pilots == null)
                    return null;

                foreach (var pilot in pilots)
                {
                    ReturnPilotCrew item = new ReturnPilotCrew
                    {
                        pilot = new ReturnPilot
                        {
                            Id = pilot.Pilot.Id,
                            FlyingHours = pilot.Pilot.FlyingHours,
                            Account = new ReturnAccount
                            {
                                Id = pilot.Pilot.Account.Id,
                                Name = pilot.Pilot.Account.Name,
                                Surname = pilot.Pilot.Account.Surname,
                                Login = pilot.Pilot.Account.Login,
                                Phone = pilot.Pilot.Account.Phone,
                                Email = pilot.Pilot.Account.Email,
                                Balance = pilot.Pilot.Account.Balance,
                                Role = new ReturnRole
                                {
                                    Id = pilot.Pilot.Account.Role.Id,
                                    Role = pilot.Pilot.Account.Role.Role1
                                }
                            }
                        },
                        position = new ReturnPosition
                        {
                            Id = pilot.CrewPosition.Id,
                            position = pilot.CrewPosition.PositionName
                        }
                    };

                    response.Add(item);
                }

                return response;
            }
        }
        public async Task AddPilotToCrewAsync(string login, int schemeId, int positionId)
        {
            await using (_db)
            {
               var pilot = _db.context.Pilots
                    .Include(x => x.Account)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Account.Login == login);

                var position = _db.context.Positions
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == positionId);

                if (pilot == null || position == null)
                    throw new Exception("Bad data!");

                Models.DB_Request.Crew crew = new Models.DB_Request.Crew
                {
                    PilotId = pilot.Id,
                    CrewPositionId = position.Id,
                    TicketSchemeId = schemeId
                };

                _db.context.Add(crew);
                _db.context.SaveChanges();
            }
        }
        public async Task DeletePilotFromCrewAsync(string login)
        {
            await using (_db)
            {
                var pilot = _db.context.Crews
                    .Include(x => x.Pilot)
                    .Include(x => x.Pilot.Account)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Pilot.Account.Login == login);

                if (pilot == null)
                    throw new Exception("Bad data!");

                _db.context.Remove(pilot);
                _db.context.SaveChanges();
            }
        }
        public async Task DeleteCrewAsync(int ticketId)
        {
            await using (_db)
            {
                var crew = _db.context.Crews
                    .ToListAsync()
                    .Result
                    .Where(x => x.TicketSchemeId == ticketId);

                if (crew == null)
                    throw new Exception("Bad data!");

                foreach (var pilot in crew)
                {
                    _db.context.Remove(pilot);
                }

                _db.context.SaveChanges();
            }
        }
    }
}
