using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;

namespace WADatabase.Administration.Managment
{
    public class RoleManagment
    {
        public async Task<IEnumerable<Models.API.Response.ReturnRole>> GetAllAsync()
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            List<Models.API.Response.ReturnRole> response = new List<Models.API.Response.ReturnRole>();

            await using (db.context)
            {
                var role = db.context.Roles
                    .ToListAsync()
                    .Result;

                if (role == null)
                    throw new Exception("Not found");
                else
                {
                    for (int i = 0; i < role.Count; i++)
                    {
                        Models.API.Response.ReturnRole item = new Models.API.Response.ReturnRole
                        {
                            Id = role[i].Id,
                            Role = role[i].Role1
                        };

                        response.Add(item);
                    }
                }

                return response;
            }
        }
    }
}
