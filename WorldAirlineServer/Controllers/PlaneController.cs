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
    public class PlaneController : ControllerBase
    {
        private PlaneManagment _db;
        public PlaneController(PlaneManagment dbClient)
        {
            _db = dbClient;
        }
        [HttpGet]
        [Route("/getAllPlanes")]
        [EnableCors("WACorsPolicy")]
        //[Authorize(Roles = "admin, moderator")]
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
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("/getPlane")]
        [EnableCors("WACorsPolicy")]
        //[Authorize(Roles = "admin, moderator, pilot, user, logistician")]
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
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("/createPlane")]
        [EnableCors("WACorsPolicy")]
        //[Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CreatePlane(ReceivedPlane incomingData)
        {
            try
            {
                await _db.CreatePlaneAsync(incomingData);

                return StatusCode(201, "Created");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete]
        [Route("/deletePlane")]
        [EnableCors("WACorsPolicy")]
        //[Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeletePlane(int id)
        {
            try
            {
                await _db.DeletePlaneAsync(id);

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
