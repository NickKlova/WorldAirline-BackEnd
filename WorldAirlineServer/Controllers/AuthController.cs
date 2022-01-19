using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AWSDatabase.Administration.Interfaces;
using AWSDatabase.Administration.Managment;
using AWSDatabase.Extensions;
using AWSDatabase.Models.AmazonResponse;
using JWTAuth.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WADatabase.Administration.Managment;
using WorldAirlineServer.Models.Auth.Request;

namespace WorldAirlineServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuth _dynamoDbClient;
        public AuthController(IAuth dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        [HttpPost]
        [Route("auth/refreshToken")]
        [EnableCors("WACorsPolicy")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokens incomingData)
        {
            var principal = TokenSetUp.GetPrincipalFromExpiredToken(incomingData.Token);

            var login = principal.Identity.Name;

            var savedRefreshToken =  _dynamoDbClient.GetTokenByLoginAsync(login).Result.refreshToken;

            if (savedRefreshToken != incomingData.RefreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = TokenSetUp.GenerateToken(principal.Claims);
            var newRefreshToken = TokenSetUp.GenerateRefreshToken();

            await _dynamoDbClient.ChangeTokenByLoginAsync(login, newRefreshToken);

            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpGet]
        [Route("auth/get/refreshToken")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetRefreshToken([FromQuery] string login)
        {
            try
            {
                var result = await _dynamoDbClient.GetTokenByLoginAsync(login);

                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                if (e.Message == "Not found!")
                    return StatusCode(404, e.Message);
                else
                    return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("auth/create/refreshToken")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateRecord([FromBody] RefreshToken incomingData)
        {
            try
            {
                await _dynamoDbClient.CreateRecord(incomingData);

                return StatusCode(201, "Created!");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPut]
        [Route("auth/update/refreshToken")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateRefreshToken([FromBody] UpdateRefreshToken incomingData)
        {
            try
            {
                await _dynamoDbClient.ChangeTokenByLoginAsync(incomingData.Login, incomingData.RefreshToken); ;

                return StatusCode(200, "Updated!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPatch]
        [Route("auth/change/login")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateLogin([FromQuery] string oldLogin, [FromQuery] string newLogin)
        {
            try
            {
                await _dynamoDbClient.ChangeLoginByLoginAsync(oldLogin, newLogin);

                return StatusCode(200, "Updated!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
        
        [HttpDelete]
        [Route("auth/delete/refreshToken")]
        [EnableCors("WACorsPolicy")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRecord([FromQuery] string login)
        {
            try
            {
                await _dynamoDbClient.DeleteRecordByLogin(login);

                return StatusCode(200, "Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
