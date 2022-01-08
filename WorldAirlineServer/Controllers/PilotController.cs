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
        [HttpGet]
        [Route("/getPilotByLogin")]
        public async Task<IActionResult> GetPilotByLogin(string login)
        {
            try
            {
                PilotManagment db = new PilotManagment();

                var response = await db.GetPilotByLogin(login);

                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/getPilotByCredentials")]
        public async Task<IActionResult> GetPilotByCredentials(string name, string surname)
        {
            try
            {
                PilotManagment db = new PilotManagment();

                var response = await db.GetPilotByCredentials(name, surname);

                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("/updateFlyingHours")]
        public async Task<IActionResult> UpdateFlyingHours(int amount, string login)
        {
            try
            {
                PilotManagment db = new PilotManagment();

                await db.UpdateFlyingHours(amount, login);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("/createPilot")]
        public async Task<IActionResult> CreatePilot(ReceivedPilot incomingData)
        {
            try
            {
                PilotManagment db = new PilotManagment();

                await db.CreatePilot(incomingData);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("/deletePilot")]
        public async Task<IActionResult> DeletePilot(string login)
        {
            try
            {
                PilotManagment db = new PilotManagment();

                await db.DeletePilotByLogin(login);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
