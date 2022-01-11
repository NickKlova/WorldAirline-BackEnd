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
    [Route("api/[controller]")]
    [ApiController]
    public class PilotController : ControllerBase
    {
        private PilotManagment _db;
        public PilotController(PilotManagment dbClient)
        {
            _db = dbClient;
        }
        [HttpGet]
        [Route("/getPilot")]
        [EnableCors("WACorsPolicy")]
        //[Authorize(Roles = "admin, moderator, ")]
        public async Task<IActionResult> GetPilot(string login)
        {
            try
            {
                var response = await _db.GetPilotAsync(login);
                if (response == null)
                    return StatusCode(404, "Not found");
                else
                    return StatusCode(200, response);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("/getPilotByPersonalInfo")]
        [EnableCors("WACorsPolicy")]
        //[Authorize(Roles = "admin, moderator, pilot, user, logistician")]
        public async Task<IActionResult> GetPilot(string name, string surname)
        {
            try
            {
                var response = await _db.GetPilotByPersonalInfo(name, surname);
                if (response == null)
                    return StatusCode(404, "Not found");
                else
                    return StatusCode(200, response);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Route("/updateFlyingHours")]
        [EnableCors("WACorsPolicy")]
        //[Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> UpdateFlyingHours(int amount, string login)
        {
            try
            {
                await _db.UpdateFlyingHoursAsync(amount, login);

                return StatusCode(200, "Updated");
            }
            catch(Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("/createPilot")]
        [EnableCors("WACorsPolicy")]
        //[Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CreatePilot(ReceivedPilot incomingData)
        {
            try
            {
                await _db.CreatePilotAsync(incomingData);

                return StatusCode(201, "Created");
            }
            catch (Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("/deletePilot")]
        [EnableCors("WACorsPolicy")]
        //[Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeletePilot(int id)
        {
            try
            {
                await _db.DeletePilotAsync(id);

                return StatusCode(200, "Deleted");
            }
            catch(Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500, e.Message);
            }
        }
    }
}
