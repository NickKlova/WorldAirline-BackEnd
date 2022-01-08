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
    public class WayController : ControllerBase
    {
        [HttpGet]
        [Route("/getAllWays")]
        public async Task<IActionResult> GetAllAirports()
        {
            try
            {
                WayManagment db = new WayManagment();

                var result = await db.GetAllWays();

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/getWayById")]
        public async Task<IActionResult> GetWay(int id)
        {
            try
            {
                WayManagment db = new WayManagment();

                var result = await db.GetWayById(id);

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("/createWay")]
        public async Task<IActionResult> CreateWay(ReceivedWay incomingData)
        {
            try
            {
                WayManagment db = new WayManagment();

                await db.CreateWay(incomingData);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("/updateActuality")]
        public async Task<IActionResult> ChangeActuality(int wayId, bool status)
        {
            try
            {
                WayManagment db = new WayManagment();

                await db.ChangeWayActuality(wayId, status);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("/deleteWay")]
        public async Task<IActionResult> DeleteWay(int wayId)
        {
            try
            {
                WayManagment db = new WayManagment();

                await db.DeleteWay(wayId);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
