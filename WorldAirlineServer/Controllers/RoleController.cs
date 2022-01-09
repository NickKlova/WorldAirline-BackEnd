using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WADatabase.Administration.Managment;

namespace WorldAirlineServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private RoleManagment _db;
        public RoleController(RoleManagment dbClient)
        {
            _db = dbClient;
        }
        [HttpGet]
        [Route("/getAllRoles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var response = await _db.GetAllRolesAsync();
                if (response == null)
                    return StatusCode(404, "Not found");
                else
                    return StatusCode(200, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
