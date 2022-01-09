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
    public class AirportController : ControllerBase
    {
        private AirportManagment _db;
        public AirportController(AirportManagment dbClient)
        {
            _db = dbClient;
        }

        [HttpGet]
        [Route("/getAirportById")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot, logistician")]
        public async Task<IActionResult> GetAirportById(int id)
        {
            try
            {
                var response = await _db.GetAirportAsync(id);

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
        [Route("/getAirportsByCountry")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot, user, logistician")]
        public async Task<IActionResult> GetAirportsByCountry(string country)
        {
            try
            {
                var response = await _db.GetAirportsAsync(country);

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
        [Route("/getAllAirports")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot, user, logistician")]
        public async Task<IActionResult> GetAllAirports()
        {
            try
            {
                var response = await _db.GetAllAirportsAsync();

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

        [HttpPost]
        [Route("/createAirport")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CreateAirport(ReceivedAirport incomingData)
        {
            try
            {
                await _db.CreateAirportAsync(incomingData);

                return StatusCode(200, "Created");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete]
        [Route("/deleteAirport")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteAirport(int id)
        {
            try
            {
                await _db.DeleteAirportAsync(id);

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
