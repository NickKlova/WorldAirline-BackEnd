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
        [HttpGet]
        [Route("/getAllAirports")]
        public async Task<IActionResult> GetAllAirports()
        {
            try
            {
                AirportManagment db = new AirportManagment();

                var result = await db.GetAllAirports();

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/getAirportByCountries")]
        public async Task<IActionResult> GetAirportsByCountries(string country)
        {
            try
            {
                AirportManagment db = new AirportManagment();

                var result = await db.GetAirportsByCountry(country);

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/getAirportById")]
        public async Task<IActionResult> GetAirportById(int id)
        {
            try
            {
                AirportManagment db = new AirportManagment();

                var result = await db.GetAirport(id);

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("/createAirport")]
        public async Task<IActionResult> CreateAirport(ReceivedAirport incomingData)
        {
            try
            {
                AirportManagment db = new AirportManagment();

                await db.CreateAirport(incomingData);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("/deleteAirport")]
        public async Task<IActionResult> DeleteAirport(int id)
        {
            try
            {
                AirportManagment db = new AirportManagment();

                await db.DeleteAirport(id);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
