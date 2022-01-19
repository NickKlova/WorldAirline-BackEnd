using AWSDatabase.Administration.Managment;
using AWSDatabase.Models.AmazonResponse;
using JWTAuth.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WADatabase.Administration.Managment;
using WADatabase.Models.API.Request;
using WorldAirlineServer.Models.Account;
using WorldAirlineServer.Models.Account.Request;
using AWSDatabase.Administration.Interfaces;
using WADatabase.Administration.Managment.Interfaces;

namespace WorldAirlineServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITicket _db;
        private readonly IAccount _accountDb;
        private readonly IAuth _dynamoDbClient;
        public UserController(ITicket dbClient, IAccount accountDbClient, IAuth dynamoDbClient)
        {
            _db = dbClient;
            _accountDb = accountDbClient;
            _dynamoDbClient = dynamoDbClient;
        }

        [HttpPost]
        [Route("user/account/login")]
        [EnableCors("WACorsPolicy")]
        public async Task<IActionResult> SignIn([FromBody] AccountLogin incomingData)
        {
            try
            {
                var encryptedPassword = HidingPassword.GetHashString(incomingData.Password);

                var identity = await _accountDb.GetIdentity(incomingData.Login, encryptedPassword);
                if (identity == null)
                {
                    return StatusCode(401, "Invalid username or password.");
                }
                else
                {
                    var awsModel = await _dynamoDbClient.GetTokenByLoginAsync(incomingData.Login);

                    string refToken = TokenSetUp.GenerateRefreshToken();
                    RefreshToken data = new RefreshToken
                    {
                        login = incomingData.Login,
                        refreshToken = refToken
                    };

                    if (awsModel != null)
                    {
                        await _dynamoDbClient.ChangeTokenByLoginAsync(awsModel.login, refToken);
                    }
                    else
                    {
                        await _dynamoDbClient.CreateRecord(data);
                    }

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
        [Route("user/account/register")]
        [EnableCors("WACorsPolicy")]
        public async Task<IActionResult> Register([FromBody] ReceivedAccount incomingData)
        {
            try
            {
                var encryptedPassword = HidingPassword.GetHashString(incomingData.Password);

                incomingData.Password = encryptedPassword;

                await _accountDb.RegisterAccountAsync(incomingData);

                var identity = await _accountDb.GetIdentity(incomingData.Login, incomingData.Password);

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
        [Route("user/account/change/login")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, user, logistician")]
        public async Task<IActionResult> ChangeLogin([FromBody] ChangeLogin incomingData)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var login = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                var encryptedPassword = HidingPassword.GetHashString(incomingData.Password);

                await _accountDb.ChangeAccountLoginAsync(login, incomingData.NewLogin, encryptedPassword);

                await _dynamoDbClient.DeleteRecordByLogin(login);

                var identity = await _accountDb.GetIdentity(incomingData.NewLogin, encryptedPassword);

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
        [Route("user/account/change/password")]
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

                await _accountDb.ChangeAccountPasswordAsync(login, encryptedOldPassword, encryptedNewPassword);

                return StatusCode(200, "Updated!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpGet]
        [Route("user/account/get/tickets/all")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, manager, pilot, user, logistician")]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string login = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                var response = await _db.GetMyTicketsAsync(login);
                if (response == null)
                    return StatusCode(404, "Not found!");
                else
                    return StatusCode(200, response);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpGet]
        [Route("user/ticket/get")]
        [EnableCors("WACorsPolicy")]
        public async Task<IActionResult> GetTicketByCode(string code)
        {
            try
            {
                var response = await _db.GetTicketByCodeAsync(code);
                if (response == null)
                    return StatusCode(404, "Not found!");
                else
                    return StatusCode(200, response);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPut]
        [Route("user/account/buy/ticket")]
        [EnableCors("WACorsPolicy")]
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
                var response = await _db.BuyTicketAsync(login, incomingData.Passenger, incomingData.Info);

                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
