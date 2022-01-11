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
        [Route("/get/allTickets/fullInfo")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager")]
        public async Task<IActionResult> GetAllTicketsFullInfo(int schemeId)
        {
            try
            {
                var response = await _db.GetFullInfoTicketsAsync(schemeId);

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
        [Route("/get/allTickets/shortInfo")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager")]
        public async Task<IActionResult> GetAllTicketsShortInfo(int schemeId)
        {
            try
            {
                var response = await _db.GetShortInfoTicketsAsync(schemeId);

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
        [Route("/get/bookedTickets/fullInfo")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager")]
        public async Task<IActionResult> GetBookedTicketsFull(int schemeId)
        {
            try
            {
                var response = await _db.GetFullInfoBookedTicketAsync(schemeId);

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
        [Route("/get/bookedTickets/shortInfo")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager")]
        public async Task<IActionResult> GetBookedTicketsShort(int schemeId)
        {
            try
            {
                var response = await _db.GetShortInfoBookedTicketAsync(schemeId);

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
        [Route("/get/unBookedTickets/fullInfo")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, logistician, user")]
        public async Task<IActionResult> GetUnBookedTicketsFull(int schemeId)
        {
            try
            {
                var response = await _db.GetFullInfoUnBookedTicketAsync(schemeId);

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
        [Route("/get/unBookedTickets/shortInfo")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, logistician, user")]
        public async Task<IActionResult> GetUnBookedTicketsShort(int schemeId)
        {
            try
            {
                var response = await _db.GetShortInfoUnBookedTicketAsync(schemeId);

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
        [Route("/get/TicketSchemeByWayId")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, logistician")]
        public async Task<IActionResult> GetTicketSchemeByWayId(int wayId)
        {
            try
            {
                var response = await _db.GetTicketSchemeByWayIdAsync(wayId);

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
        [Route("/get/TicketShemeById")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, logistician")]
        public async Task<IActionResult> GetTicketShemeById(int id)
        {
            try
            {
                var response = await _db.GetTicketSchemeByIdAsync(id);

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
        [Route("/create/ticketSheme")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CreateTicketScheme(ReceiveTicketScheme ticketScheme)
        {
            try
            {
                await _db.CreateTicketSchemeAsync(ticketScheme);

                return StatusCode(201, "Created");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
        
        [HttpPost]
        [Route("/create/tickets")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager")]
        public async Task<IActionResult> CreateTickets(ReceivedTicket incomingTicket)
        {
            try
            {
                await _db.CreateTicketsAsync(incomingTicket);

                return StatusCode(201, "Created");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPatch]
        [Route("/update/flightStatusByWayId")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> UpdateFlightStatusBySchemeId(int schemeId, bool status)
        {
            try
            {
                await _db.FlightStatusChangeBySchemeIdAsync(schemeId, status);

                return StatusCode(200, "Updated!");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
        
        [HttpPatch]
        [Route("/update/flightStatusById")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> UpdateFlightStatusById(int id, bool status)
        {
            try
            {
                await _db.FlightStatusChangeByIdAsync(id, status);

                return StatusCode(200, "Updated!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
        
        [HttpPatch]
        [Route("/update/planeInTicketScheme")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> UpdatePlaneInTicketScheme(int schemeId, int planeId)
        {
            try
            {
                await _db.UpdateTicketShemePlaneAsync(schemeId, planeId);

                return StatusCode(200, "Updated!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
        
        [HttpPatch]
        [Route("/update/priceInTicket")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager")]
        public async Task<IActionResult> UpdatePriceInTicket(int schemeId, decimal price)
        {
            try
            {
                await _db.UpdateTicketPriceAsync(schemeId, price);

                return StatusCode(200, "Updated!");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete]
        [Route("/delete/tickets")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteTickets(int ticketScheme)
        {
            try
            {
                await _db.DeleteTicketsAsync(ticketScheme);

                return StatusCode(200, "Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
        
        [HttpDelete]
        [Route("/delete/ticket")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            try
            {
                await _db.DeleteTicketAsync(id);

                return StatusCode(200, "Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
        
        [HttpDelete]
        [Route("/delete/ticketScheme")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteTicketScheme(int id)
        {
            try
            {
                await _db.DeleteTicketSchemeAsync(id);

                return StatusCode(200, "Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
