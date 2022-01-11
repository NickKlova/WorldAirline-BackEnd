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
    public class PassengerController : ControllerBase
    {
        private PassengerManagment _db;
        public PassengerController(PassengerManagment dbClient)
        {
            _db = dbClient;
        }
        [HttpGet]
        [Route("passenger/get")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> GetPassengerByPassportSeries([FromQuery] int id)
        {
            try
            {
                var response = await _db.GetPassengerAsync(id);

                if (response == null)
                    return StatusCode(404, "Not found!");
                else
                    return StatusCode(200, response);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("passenger/get/bySurname")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> GetPassengerBySurname([FromQuery] string surname)
        {
            try
            {
                var response = await _db.GetPassengersBySurnameAsync(surname);

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
        [Route("passenger/create")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, user")]
        public async Task<IActionResult> CreatePassenger([FromBody] ReceivedPassenger incomingData)
        {
            try
            {
                await _db.CreatePassengerAsync(incomingData);

                return StatusCode(201, "Created");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete]
        [Route("passenger/delete")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeletePassenger([FromQuery] int id)
        {
            try
            {
                await _db.DeletePassengerAsync(id);

                return StatusCode(200, "Deleted");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
