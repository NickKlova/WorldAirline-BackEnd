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
        private TicketManagment _ticketDb;
        public AccountController(AuthManagment dynamoDbClient, AccountManagment dbClient, TicketManagment ticketDbClient)
        {
            _db = dbClient;
            _dynamoDbClient = dynamoDbClient;
            _ticketDb = ticketDbClient;
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

        [HttpPut]
        [Route("account/buy/ticket")]
        public async Task<IActionResult> BuyTicket([FromBody] BuyTicket incomingData)
        {
            try
            {
                string login = null;
                if (User.Identity.IsAuthenticated)
                {
                    var claimsIdentity = this.User.Identity as ClaimsIdentity;
                    login = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                }
                var response = await _ticketDb.BuyTicketAsync(login, incomingData.Passenger, incomingData.Info);

                return StatusCode(200, response);
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPost]
        [Route("account/login")]
        [EnableCors("WACorsPolicy")]
        public async Task<IActionResult> SignUp([FromBody] AccountLogin incomingData)
        {
            try
            {
                var encryptedPassword = HidingPassword.GetHashString(incomingData.Password);

                var identity = await _db.GetIdentity(incomingData.Login, encryptedPassword);
                if (identity == null)
                {
                  return StatusCode(401, "Invalid username or password.");
                }
                else
                {
                    var awsModel = await _dynamoDbClient.GetTokenByLoginAsync(incomingData.Login);
                    string refToken;
                    if (awsModel == null)
                    {
                        refToken = TokenSetUp.GenerateRefreshToken();
                    }
                    else
                    {
                        refToken = awsModel.refreshToken;
                    }

                    RefreshToken data = new RefreshToken
                    {
                        login = incomingData.Login,
                        refreshToken = refToken
                    };

                    var response = new TokenResponse
                    {
                        Token = TokenSetUp.GenerateToken(identity.Claims),
                        RefreshToken = refToken
                    };
                    return StatusCode(200, response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPost]
        [Route("account/register")]
        [EnableCors("WACorsPolicy")]
        public async Task<IActionResult> Register([FromBody] ReceivedAccount incomingData)
        {
            try
            {
                var encryptedPassword = HidingPassword.GetHashString(incomingData.Password);

                incomingData.Password = encryptedPassword;

                await _db.RegisterAccountAsync(incomingData);

                var identity = await _db.GetIdentity(incomingData.Login, incomingData.Password);

                var response = new TokenResponse
                {
                    Token = TokenSetUp.GenerateToken(identity.Claims),
                    RefreshToken = TokenSetUp.GenerateRefreshToken()
                };

                RefreshToken data = new RefreshToken
                {
                    login = incomingData.Login,
                    refreshToken = response.RefreshToken
                };

                await _dynamoDbClient.CreateRecord(data);

                return StatusCode(201, response);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPut]
        [Route("account/change/login")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, user, logistician")]
        public async Task<IActionResult> ChangeLogin([FromBody] ChangeLogin incomingData)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var login = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                var encryptedPassword = HidingPassword.GetHashString(incomingData.Password);

                await _db.ChangeAccountLoginAsync(login, incomingData.NewLogin, encryptedPassword);

                await _dynamoDbClient.DeleteRecordByLogin(login);

                var identity = await _db.GetIdentity(incomingData.NewLogin, encryptedPassword);

                var response = new TokenResponse
                {
                    Token = TokenSetUp.GenerateToken(identity.Claims),
                    RefreshToken = TokenSetUp.GenerateRefreshToken()
                };

                RefreshToken data = new RefreshToken
                {
                    login = incomingData.NewLogin,
                    refreshToken = response.RefreshToken
                };

                await _dynamoDbClient.CreateRecord(data);

                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPut]
        [Route("account/change/password")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, user, logistician")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword incomingData)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var login = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                var encryptedOldPassword = HidingPassword.GetHashString(incomingData.OldPassword);
                var encryptedNewPassword = HidingPassword.GetHashString(incomingData.NewPassword);

                await _db.ChangeAccountPasswordAsync(login, encryptedOldPassword, encryptedNewPassword);

                return StatusCode(200, "Updated!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
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
