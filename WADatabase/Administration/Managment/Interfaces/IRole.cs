using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment.Interfaces
{
    public interface IRole
    {
        public Task<IEnumerable<Models.API.Response.ReturnRole>> GetAllRolesAsync();
        public Task GiveRoleAsync(string login, string role);
    }
}
