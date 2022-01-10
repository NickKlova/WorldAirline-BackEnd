using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    public class TicketController : ControllerBase
    {
        private TicketManagment _db;
        public TicketController(TicketManagment dbClient)
        {
            _db = dbClient;
        }
        [HttpGet]
        [Route("/getTicketSchemeByWayId")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot, logistician")]
        public async Task<IActionResult> GetTicketSchemeByWayId(int wayId)
        {
            try
            {
                var response = await _db.GetTicketSchemeByWayIdAsync(wayId);
                if (response == null)
                    return StatusCode(404, "Not found");
                else
                    return StatusCode(200, response);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("/getTicketShemeById")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot, logistician")]
        public async Task<IActionResult> GetTicketShemeById(int id)
        {
            try
            {
                var response = await _db.GetTicketSchemeByIdAsync(id);
                if (response == null)
                    return StatusCode(404, "Not found");
                else
                    return StatusCode(200, response);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("/createTicketSheme")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CreateTicketScheme(ReceiveTicketScheme ticketScheme)
        {
            try
            {
                await _db.CreateTicketSchemeAsync(ticketScheme);

                return StatusCode(200, "Created");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPut]
        [Route("/updateFlightStatusByWayId")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> UpdateFlightStatusByWayId(int wayId, DateTime departureDate, bool status)
        {
            try
            {
                await _db.FlightStatusChangeAsync(wayId, departureDate, status);

                return StatusCode(200, "Updated");
            }
            catch(Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Route("/updateFlightStatusById")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> UpdateFlightStatusById(int id, bool status)
        {
            try
            {
                await _db.FlightStatusChangeAsync(id, status);

                return StatusCode(200, "Updated");
            }
            catch (Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Route("/updatePlaneInTicketScheme")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> UpdatePlaneInTicketScheme(int wayId, int planeId)
        {
            try
            {
                await _db.UpdateTicketShemePlaneAsync(wayId, planeId);

                return StatusCode(200, "Updated");
            }
            catch (Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("/createTicket")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CreateTicket(int ticketAmount, string travelClass, decimal price, ReceivedTicket incomingTicket)
        {
            try
            {
                await _db.CreateTicketsAsync(ticketAmount, travelClass, price, incomingTicket);

                return StatusCode(200, "Created");
            }
            catch (Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("/deleteTicket")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteTicket(DateTime departureDate, DateTime arrivalDate, int ticketScheme)
        {
            try
            {
                await _db.DeleteTicketsAsync(departureDate, arrivalDate, ticketScheme);

                return StatusCode(200, "Deleted");
            }
            catch (Exception e)
            {
                if (e.Message == "Bad data!")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500, e.Message);
            }
        }
    }
}
