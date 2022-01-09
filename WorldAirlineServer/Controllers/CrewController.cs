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

namespace WorldAirlineServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrewController : ControllerBase
    {
        private CrewManagment _db;
        public CrewController(CrewManagment dbClient)
        {
            _db = dbClient;
        }

        [HttpGet]
        [Route("/getCrew")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> GetCrewByTicketId(int ticketId)
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
        [Route("/addPilotToTheCrew")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> AddPilot(string pilotLogin, int ticketId, string position)
        {
            try
            {
                await _db.AddPilotToCrewAsync(pilotLogin, ticketId, position);

                return StatusCode(200);
            }
            catch(Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("/deletePilotFromTheCrew")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeletePilotFromTheCrew(string login)
        {
            try
            {
                await _db.DeletePilotFromCrewAsync(login);

                return StatusCode(200);
            }
            catch(Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("/deleteCrew")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CrewDelete(int ticketId)
        {
            try
            {
                await _db.DeleteCrewAsync(ticketId);

                return StatusCode(200);
            }
            catch(Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500);
            }
        }
    }
}
