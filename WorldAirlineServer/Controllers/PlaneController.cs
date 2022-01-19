using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WADatabase.Administration.Managment;
using WADatabase.Administration.Managment.Interfaces;
using WADatabase.Models.API.Request;

namespace WorldAirlineServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PlaneController : ControllerBase
    {
        private readonly IPlane _db;
        public PlaneController(IPlane dbClient)
        {
            _db = dbClient;
        }
        [HttpGet]
        [Route("plane/get/all")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot")]
        public async Task<IActionResult> GetAllPlanes()
        {
            try
            {
                var response = await _db.GetAllPlanesAsync();
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
        [Route("plane/get")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot")]
        public async Task<IActionResult> GetPlane(int id)
        {
            try
            {
                var response = await _db.GetPlaneAsync(id);
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
        [Route("plane/create")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CreatePlane(ReceivedPlane incomingData)
        {
            try
            {
                await _db.CreatePlaneAsync(incomingData);

                return StatusCode(201, "Created!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete]
        [Route("plane/delete")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeletePlane(int id)
        {
            try
            {
                await _db.DeletePlaneAsync(id);

                return StatusCode(200, "Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
