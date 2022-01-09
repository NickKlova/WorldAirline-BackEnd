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
    [Route("api/[controller]")]
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
        [Route("/getAccountById")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> GetAccount(int id)
        {
            try
            {
                var response = await _db.GetAccountAsync(id);

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
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> GetAccount(string login)
        {
            try
            {
                var response = await _db.GetAccountAsync(login);

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

        [HttpGet]
        [Route("/signUpAccount")]
        [EnableCors("WACorsPolicy")]
        public async Task<IActionResult> LoginAccount([FromBody] AccountLogin incomingData)
        {
            try
            {
                var encryptedPassword = HidingPassword.GetHashString(incomingData.Password);

                var identity = await _db.GetIdentity(incomingData.Login, encryptedPassword);
                if (identity == null)
                {
                    throw new Exception("Invalid username or password.");
                }
                else
                {
                    var awsModel = await _dynamoDbClient.GetTokenByLoginAsync(incomingData.Login);

                    RefreshToken data = new RefreshToken
                    {
                        login = incomingData.Login,
                        refreshToken = awsModel.refreshToken
                    };

                    var response = new TokenResponse
                    {
                        Token = TokenSetUp.GenerateToken(identity.Claims),
                        RefreshToken = awsModel.refreshToken
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
        [Route("/registerAccount")]
        [EnableCors("WACorsPolicy")]
        public async Task<IActionResult> CreateAccount([FromBody] ReceivedAccount incomingData)
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
        [Route("/changeLogin")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot, user, logistician")]
        public async Task<IActionResult> ChangeLogin([FromBody] ChangeLogin incomingData)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var login = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                var encryptedPassword = HidingPassword.GetHashString(incomingData.Password);

                await _db.ChangeAccountLoginAsync(login, incomingData.NewLogin, encryptedPassword);

                await _dynamoDbClient.DeleteRecordByLogin(login);

                var identity = await _db.GetIdentity(incomingData.NewLogin, incomingData.Password);

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

                return StatusCode(201, response);
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
        [Route("/changePassword")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator, pilot, user, logistician")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword incomingData)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var login = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                var encryptedOldPassword = HidingPassword.GetHashString(incomingData.OldPassword);
                var encryptedNewPassword = HidingPassword.GetHashString(incomingData.NewPassword);

                await _db.ChangeAccountPasswordAsync(login, encryptedOldPassword, encryptedNewPassword);

                return StatusCode(201, "Updated");
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
        [Route("/deleteAccount")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteAccount(string login)
        {
            try
            {
                await _dynamoDbClient.DeleteRecordByLogin(login);

                await _db.DeleteAccountAsync(login); 

                return StatusCode(201, "Deleted");
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
