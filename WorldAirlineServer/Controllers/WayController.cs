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
    public class WayController : ControllerBase
    {
        private WayManagment _db;
        public WayController(WayManagment dbClient)
        {
            _db = dbClient;
        }
        [HttpGet]
        [Route("way/get/all")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, logistician, user")]
        public async Task<IActionResult> GetAllWays()
        {
            try
            {
                var response = await _db.GetAllWaysAsync();
                if (response == null)
                    return StatusCode(404, "Not found!");
                else
                    return StatusCode(200, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("way/get")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot, logistician, manager")]
        public async Task<IActionResult> GetWay(int id)
        {
            try
            {
                var response = await _db.GetWayAsync(id);
                if (response == null)
                    return StatusCode(404, "Not found!");
                else
                    return StatusCode(200, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("way/create")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, logistician")]
        public async Task<IActionResult> CreateWay(ReceivedWay incomingData)
        {
            try
            {
                await _db.CreateWayAsync(incomingData);

                return StatusCode(201, "Created!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPatch]
        [Route("way/update/actuality")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot, logistician")]
        public async Task<IActionResult> ChangeActuality(int wayId, bool status)
        {
            try
            {
                await _db.ChangeWayActualityAsync(wayId, status);

                return StatusCode(200, "Updated!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete]
        [Route("way/delete")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, logistician")]
        public async Task<IActionResult> DeleteWay(int wayId)
        {
            try
            {
                await _db.DeleteWayAsync(wayId);

                return StatusCode(200, "Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
