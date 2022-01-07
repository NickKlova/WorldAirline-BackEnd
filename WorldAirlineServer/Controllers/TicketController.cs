using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WADatabase.Administration.Managment;
using WADatabase.Models.API.Request;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldAirlineServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        [HttpGet]
        [Route("/getTicketSchemeByWayId")]
        public async Task<IActionResult> GetTicketSchemeByWayId(int wayId)
        {
            try
            {
                TicketManagment db = new TicketManagment();

                var result = await db.GetTicketSchemeByWayId(wayId);

                return StatusCode(200, result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/getTicketShemeById")]
        public async Task<IActionResult> GetTicketShemeById(int id)
        {
            try
            {
                TicketManagment db = new TicketManagment();

                var result = await db.GetTicketSchemeById(id);

                return StatusCode(200, result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("/createTicketSheme")]
        public async Task<IActionResult> CreateTicketScheme(ReceiveTicketScheme ticketScheme)
        {
            try
            {
                TicketManagment db = new TicketManagment();

                await db.CreateTicketSchemeAsync(ticketScheme);

                return StatusCode(200);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("/updateFlightStatusByWayId")]
        public async Task<IActionResult> UpdateFlightStatusByWayId(int wayId, DateTime departureDate, bool status)
        {
            try
            {
                TicketManagment db = new TicketManagment();

                await db.FlightStatusChangeByWayAsync(wayId, departureDate, status);

                return StatusCode(200);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("/updateFlightStatusById")]
        public async Task<IActionResult> UpdateFlightStatusById(int id, bool status)
        {
            try
            {
                TicketManagment db = new TicketManagment();
                await db.FlightStatusChangeByIdAsync(id, status);

                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        [HttpPut]
        [Route("/updatePlaneInTicketScheme")]
        public async Task<IActionResult> UpdatePlaneInTicketScheme(int wayId, int planeId)
        {
            try
            {
                TicketManagment db = new TicketManagment();

                await db.UpdateTicketShemePlaneByWayIdAsync(wayId, planeId);

                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        [HttpPost]
        [Route("/createTicket")]
        public async Task<IActionResult> CreateTicket(int ticketAmount, string travelClass, decimal price, ReceivedTicket incomingTicket)
        {
            try
            {
                TicketManagment db = new TicketManagment();

                await db.CreateTicketsAsync(ticketAmount, travelClass, price, incomingTicket);

                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        [HttpDelete]
        [Route("/deleteTicket")]
        public async Task<IActionResult> DeleteTicket(DateTime departureDate, DateTime arrivalDate, int ticketScheme)
        {
            try
            {
                TicketManagment db = new TicketManagment();

                await db.DeleteTicketsAsync(departureDate, arrivalDate, ticketScheme);

                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }
    }
}
