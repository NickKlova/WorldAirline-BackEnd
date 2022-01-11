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
    public class AirportController : ControllerBase
    {
        private AirportManagment _db;
        public AirportController(AirportManagment dbClient)
        {
            _db = dbClient;
        }

        [HttpGet]
        [Route("airport/get/byId")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, logistician")]
        public async Task<IActionResult> GetAirport([FromQuery] int id)
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
        [Route("airport/get/byCountry")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot, manager, user, logistician")]
        public async Task<IActionResult> GetAirport([FromQuery] string country)
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
        [Route("airport/get/all")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, user, logistician")]
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
        [Route("airport/create")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CreateAirport([FromBody] ReceivedAirport incomingData)
        {
            try
            {
                await _db.CreateAirportAsync(incomingData);

                return StatusCode(201, "Created!");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete]
        [Route("airport/delete")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteAirport(int id)
        {
            try
            {
                await _db.DeleteAirportAsync(id);

                return StatusCode(200, "Deleted!");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
