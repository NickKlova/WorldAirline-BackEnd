using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class AccountController : ControllerBase
    {
        [HttpGet]
        [Route("/getAccountById")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            try
            {
                AccountManagment account = new AccountManagment();
                var response = await account.GetByIdAsync(id);
                if (response != null)
                    return StatusCode(200, response);
                else
                    return StatusCode(404, "Not found");
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("/getAccountByLogin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAccountByLogin(string login)
        {
            try
            {
                AccountManagment account = new AccountManagment();
                var response = await account.GetByLoginAsync(login);
                if (response != null)
                    return StatusCode(200, response);
                else
                    return StatusCode(404, "Not found");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("/createAccount")]
        [EnableCors("WACorsPolicy")]
        public async Task<IActionResult> CreateAccount([FromBody] ReceivedAccount incomingData)
        {
            try
            {
                AccountManagment account = new AccountManagment();
                await account.RegistrationAsync(incomingData);
                
                return StatusCode(201, "Registrated");
            }
            catch (Exception e)
            {
                if (e.Message == "Bad request")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Route("/changeBalance")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeBalace(decimal amount, string login)
        {
            try
            {
                AccountManagment account = new AccountManagment();
                await account.ChangeBalanceAsync(amount, login);

                return StatusCode(201, "Updated");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPut]
        [Route("/changeLogin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeLogin(string oldLogin, string newLogin)
        {
            try
            {
                AccountManagment account = new AccountManagment();
                await account.ChangeLoginAsync(oldLogin, newLogin);

                return StatusCode(201, "Updated");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPut]
        [Route("/changePassword")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePassword(string login, string password)
        {
            try
            {
                AccountManagment account = new AccountManagment();
                await account.ChangePasswordAsync(login, password);

                return StatusCode(201, "Updated");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPut]
        [Route("/giveRole")]
        public async Task<IActionResult> GiveRole(string role, string login)
        {
            try
            {
                AccountManagment account = new AccountManagment();
                await account.GiveRole(role, login);

                return StatusCode(201, "Updated");
            }
            catch(Exception e)
            {
                return StatusCode(404, e.Message);
            }
        }

        [HttpDelete]
        [Route("/deleteAccount")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAccount(string login)
        {
            try
            {
                AccountManagment account = new AccountManagment();
                await account.DeleteByLoginAsync(login);

                return StatusCode(201, "Deleted");
            }
            catch (Exception e)
            {
                if (e.Message == "Bad request")
                    return StatusCode(400, e.Message);
                else
                    return StatusCode(500, e.Message);
            }
        }
    }
}
