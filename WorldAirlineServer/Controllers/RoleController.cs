using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WADatabase.Administration.Managment;
using WADatabase.Administration.Managment.Interfaces;

namespace WorldAirlineServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRole _db;
        public RoleController(IRole dbClient)
        {
            _db = dbClient;
        }
        [HttpGet]
        [Route("role/get/all")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var response = await _db.GetAllRolesAsync();
                if (response == null)
                    return StatusCode(404, "Not found!");
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
