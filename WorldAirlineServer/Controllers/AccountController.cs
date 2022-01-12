using AWSDatabase.Administration.Managment;
using AWSDatabase.Models.AmazonResponse;
using JWTAuth.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WADatabase.Administration.Managment;
using WADatabase.Models.API.Request;
using WorldAirlineServer.Models.Account;
using WorldAirlineServer.Models.Account.Request;

namespace WorldAirlineServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountManagment _db;
        private AuthManagment _dynamoDbClient;
        public AccountController(AuthManagment dynamoDbClient, AccountManagment dbClient)
        {
            _db = dbClient;
            _dynamoDbClient = dynamoDbClient;
        }
        [HttpGet]
        [Route("account/get/byId")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> GetAccount([FromQuery] int id)
        {
            try
            {
                var response = await _db.GetAccountAsync(id);

                if (response != null)
                    return StatusCode(200, response);
                else
                    return StatusCode(404, "Not found!");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("account/get/byLogin")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> GetAccount([FromQuery] string login)
        {
            try
            {
                var response = await _db.GetAccountAsync(login);

                if (response != null)
                    return StatusCode(200, response);
                else
                    return StatusCode(404, "Not found!");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("account/delete")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteAccount(string login)
        {
            try
            {
                await _dynamoDbClient.DeleteRecordByLogin(login);

                await _db.DeleteAccountAsync(login);

                return StatusCode(200, "Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
