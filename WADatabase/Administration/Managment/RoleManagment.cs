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
    public class RoleManagment : Interfaces.IRole
    {
        private WorldAirlinesClient _db;
        public RoleManagment(WorldAirlinesClient dbClient)
        {
            _db = dbClient;
        }
        public async Task<IEnumerable<ReturnRole>> GetAllRolesAsync()
        {
            await using (_db)
            {
                var roles = _db.context.Roles
                    .ToListAsync()
                    .Result;

                if (roles == null)
                    return null;

                List<ReturnRole> response = new List<ReturnRole>();

                foreach (var role in roles)
                {
                    ReturnRole item = new ReturnRole
                    {
                        Id = role.Id,
                        Role = role.Role1
                    };

                    response.Add(item);
                }

                return response;
            }
        }
    }
}
