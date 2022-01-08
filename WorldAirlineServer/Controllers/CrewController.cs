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
        [HttpGet]
        [Route("/getCrewByTicketScheme")]
        public async Task<IActionResult> GetCrewByTicketId(int ticketId)
        {
            try
            {
                CrewManagment db = new CrewManagment();

                var result = await db.GetCrewByTicketScheme(ticketId);

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("/addPilotToTheCrew")]
        public async Task<IActionResult> AddPilot(int pilotId, int ticketId, int positionId)
        {
            try
            {
                CrewManagment db = new CrewManagment();

                await db.AddPilotToCrew(pilotId, ticketId, positionId);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("/deletePilotFromCrew")]
        public async Task<IActionResult> DeletePilotFromCrew(string login)
        {
            try
            {
                CrewManagment db = new CrewManagment();

                await db.DeletePilotFromCrew(login);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("/deleteCrew")]
        public async Task<IActionResult> CrewDelete(int ticketId)
        {
            try
            {
                CrewManagment db = new CrewManagment();

                await db.GetCrewByTicketScheme(ticketId);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
