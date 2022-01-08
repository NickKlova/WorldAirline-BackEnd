using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;

namespace WADatabase.Administration.Managment
{
    public class CrewManagment
    {
        public async Task AddPilotToCrew(int pilotId, int schemeId, int positionId)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                Models.DB_Request.Crew crew = new Models.DB_Request.Crew
                {
                    PilotId = pilotId,
                    CrewPositionId = positionId,
                    TicketSchemeId = schemeId
                };

                db.context.Add(crew);
            }
        }

        public async Task<IEnumerable<Models.API.Response.ReturnPilotCrew>> GetCrewByTicketScheme(int ticketId)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                List<Models.API.Response.ReturnPilotCrew> response = new List<Models.API.Response.ReturnPilotCrew>();

                var pilots = db.context.Crews
                    .Include(x => x.CrewPosition)
                    .Include(x => x.Pilot)
                    .Include(x => x.Pilot.Account)
                    .Include(x => x.Pilot.Account.Role)
                    .ToListAsync()
                    .Result
                    .Where(x => x.TicketSchemeId == ticketId);

                foreach (var pilot in pilots)
                {
                    Models.API.Response.ReturnPilotCrew item = new Models.API.Response.ReturnPilotCrew
                    {
                        pilot = new Models.API.Response.ReturnPilot
                        {
                            Id = pilot.Pilot.Id,
                            FlyingHours = pilot.Pilot.FlyingHours,
                            Account = new Models.API.Response.ReturnAccount
                            {
                                Id = pilot.Pilot.Account.Id,
                                Name = pilot.Pilot.Account.Name,
                                Surname = pilot.Pilot.Account.Surname,
                                Login = pilot.Pilot.Account.Login,
                                Phone = pilot.Pilot.Account.Phone,
                                Email = pilot.Pilot.Account.Email,
                                Balance = pilot.Pilot.Account.Balance,
                                Role = new Models.API.Response.ReturnRole
                                {
                                    Id = pilot.Pilot.Account.Role.Id,
                                    Role = pilot.Pilot.Account.Role.Role1
                                }
                            }
                        },
                        position = new Models.API.Response.ReturnPosition
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
        public async Task DeletePilotFromCrew(string login)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var pilot = db.context.Crews
                    .Include(x => x.Pilot)
                    .Include(x => x.Pilot.Account)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Pilot.Account.Login == login);

                db.context.Remove(pilot);
                db.context.SaveChanges();
            }
        }

        public async Task DeleteCrew(int ticketId)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var crew = db.context.Crews
                    .ToListAsync()
                    .Result
                    .Where(x => x.TicketSchemeId == ticketId);

                foreach(var pilot in crew)
                {
                    db.context.Remove(pilot);
                }

                db.context.SaveChanges();
            }

        }
    }
}
