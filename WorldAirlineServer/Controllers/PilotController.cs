using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WADatabase.Administration.Managment;
using WADatabase.Models.API.Request;

namespace WorldAirlineServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PilotController : ControllerBase
    {
        private PilotManagment _db;
        public PilotController(PilotManagment dbClient)
        {
            _db = dbClient;
        }

        [HttpGet]
        [Route("pilot/get")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot")]
        public async Task<IActionResult> GetPilot([FromQuery] string login)
        {
            try
            {
                var response = await _db.GetPilotAsync(login);

                if (response == null)
                    return StatusCode(404, "Not found!");
                else
                    return StatusCode(200, response);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("pilot/get/byPersonalInfo")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot")]
        public async Task<IActionResult> GetPilot([FromQuery] string name, [FromQuery] string surname)
        {
            try
            {
                var response = await _db.GetPilotByPersonalInfo(name, surname);
                if (response == null)
                    return StatusCode(404, "Not found!");
                else
                    return StatusCode(200, response);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("pilot/update/flyingHours")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> UpdateFlyingHours([FromQuery] int amount, [FromQuery] string login)
        {
            try
            {
                await _db.UpdateFlyingHoursAsync(amount, login);

                return StatusCode(200, "Updated!");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPost]
        [Route("pilot/create")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CreatePilot([FromBody] ReceivedPilot incomingData)
        {
            try
            {
                await _db.CreatePilotAsync(incomingData);

                return StatusCode(201, "Created!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete]
        [Route("pilot/delete")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeletePilot([FromQuery] int id)
        {
            try
            {
                await _db.DeletePilotAsync(id);

                return StatusCode(200, "Deleted!");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
