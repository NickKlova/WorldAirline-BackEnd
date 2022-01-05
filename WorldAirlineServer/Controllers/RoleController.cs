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
        [HttpGet]
        [Route("/getRoles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                RoleManagment role = new RoleManagment();
                var response = await role.GetAllAsync();

                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                if (e.Message == "Not found")
                    return StatusCode(404, "Not found");
                else
                    return StatusCode(500, e.Message);
            }
        }
    }
}
