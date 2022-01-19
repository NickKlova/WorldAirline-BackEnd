using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;
using WADatabase.Administration.Managment;
using WADatabase.Administration.Managment.Interfaces;

namespace WorldAirlineServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CrewController : ControllerBase
    {
        private readonly ICrew _db;
        public CrewController(ICrew dbClient)
        {
            _db = dbClient;
        }

        [HttpGet]
        [Route("crew/get")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot")]
        public async Task<IActionResult> GetCrew([FromQuery] int ticketId)
        {
            try
            {
                var response = await _db.GetCrewByTicketSchemeAsync(ticketId);

                if (response == null)
                    return StatusCode(404, "Not found!");
                else
                    return StatusCode(200, response);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        [HttpPost]
        [Route("crew/add/pilot")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> AddPilot([FromQuery] string pilotLogin, [FromQuery] int ticketId, [FromQuery] int positionId)
        {
            try
            {
                await _db.AddPilotToCrewAsync(pilotLogin, ticketId, positionId);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete]
        [Route("crew/delete/pilot")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeletePilotFromTheCrew([FromQuery] string login)
        {
            try
            {
                await _db.DeletePilotFromCrewAsync(login);

                return StatusCode(200);
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete]
        [Route("crew/delete/crew")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CrewDelete([FromQuery] int ticketId)
        {
            try
            {
                await _db.DeleteCrewAsync(ticketId);

                return StatusCode(200);
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
