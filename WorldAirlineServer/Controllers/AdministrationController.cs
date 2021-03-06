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

namespace WorldAirlineServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IRole _roleDb;
        private readonly IAccount _accountDb;
        public AdministrationController(IRole roleDbClient, IAccount accountDbClient)
        {
            _roleDb = roleDbClient;
            _accountDb = accountDbClient;
        }
        [HttpPatch]
        [Route("/admin/change/balance")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeBalance(string login, decimal amount)
        {
            try
            {
                await _accountDb.ChangeAccountBalanceAsync(amount, login);

                return StatusCode(200, "Updated!");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPatch]
        [Route("/admin/give/permissions")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GivePermissions([FromQuery] string login, [FromQuery] string role)
        {
            try
            {
                await _roleDb.GiveRoleAsync(login, role);

                return StatusCode(200, "Updated!");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
