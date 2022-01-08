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
    public class PassengerController : ControllerBase
    {
        [HttpGet]
        [Route("/getPassengerByPassportSeries")]
        public async Task<IActionResult> GetPassengerByPassportSeries(string passportSeries)
        {
            try
            {
                PassengerManagment db = new PassengerManagment();

                var result = await db.GetPassengerByPassportSeriesAsync(passportSeries);

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/getPassengerBySurname")]
        public async Task<IActionResult> GetPassengerBySurname(string surname)
        {
            try
            {
                PassengerManagment db = new PassengerManagment();

                var result = await db.GetPassengerBySurname(surname);

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("/createPassenger")]
        public async Task<IActionResult> CreatePassenger(ReceivedPassenger incomingData)
        {
            try
            {
                PassengerManagment db = new PassengerManagment();

                await db.CreatePassengerAsync(incomingData);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("/deletePassengerByPassportSeries")]
        public async Task<IActionResult> DeletePassenger(string passportSeries)
        {
            try
            {
                PassengerManagment db = new PassengerManagment();

                await db.DeletePassengerByPassportSeries(passportSeries);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
