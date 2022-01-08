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
        [HttpGet]
        [Route("/getAllPlanes")]
        public async Task<IActionResult> GetAllPlanes()
        {
            try
            {
                PlaneManagment db = new PlaneManagment();

                var result = await db.GetAllPlanes();

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/getPlanesById")]
        public async Task<IActionResult> GetPlanesById(int id)
        {
            try
            {
                PlaneManagment db = new PlaneManagment();

                var result = await db.GetPlaneById(id);

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("/createPlane")]
        public async Task<IActionResult> CreatePlane(ReceivedPlane incomingData)
        {
            try
            {
                PlaneManagment db = new PlaneManagment();

                await db.CreatePlane(incomingData);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("/deletePlane")]
        public async Task<IActionResult> DeletePlane(int id)
        {
            try
            {
                PlaneManagment db = new PlaneManagment();

                await db.DeletePlane(id);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
