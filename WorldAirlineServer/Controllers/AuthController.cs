using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AWSDatabase.Administration.Managment;
using AWSDatabase.Extensions;
using AWSDatabase.Models.AmazonResponse;
using JWTAuth.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WADatabase.Administration.Managment;

namespace WorldAirlineServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly AuthManagment _dynamoDbClient;
        public AuthController(AuthManagment dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        [HttpPost]
        [Route("/getToken")]
        public async Task<IActionResult> Token(string login, string password)
        {
            AccountManagment account = new AccountManagment();

            var identity = await account.GetIdentity(login, password);
            if (identity == null)
            {
                return BadRequest("Invalid username or password.");
            }
            else
            {
                var refreshToken = TokenSetUp.GenerateRefreshToken();

                RefreshToken data = new RefreshToken
                {
                    login = login,
                    refreshToken = refreshToken
                };

                await _dynamoDbClient.CreateRecord(data);

                var jwt = TokenSetUp.GenerateToken(identity.Claims);

                var response = new
                {
                    access_token = jwt,
                    refresh_token = refreshToken,
                    username = identity.Name
                };

                return Ok(response);
            }
        }

        [HttpPost]
        [Route("/refreshToken")]
        public async Task<IActionResult> Refresh(string token, string refreshToken)
        {
            var principal = TokenSetUp.GetPrincipalFromExpiredToken(token);

            var login = principal.Identity.Name;

            var savedRefreshToken =  _dynamoDbClient.GetTokenByLoginAsync(login).Result.refreshToken;

            if (savedRefreshToken != refreshToken)
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
        [Route("/getRefreshTokenByLogin")]
        public async Task<IActionResult> GetRefreshToken(string login)
        {
            try
            {
                var result = await _dynamoDbClient.GetTokenByLoginAsync(login);

                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                if (e.Message == "Not found")
                    return StatusCode(404, e.Message);
                else
                    return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("/createRefreshTokenRecord")]
        public async Task<IActionResult> CreateRecord(RefreshToken incomingData)
        {
            try
            {
                await _dynamoDbClient.CreateRecord(incomingData);

                return StatusCode(201, "Created");
            }
            catch(Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPut]
        [Route("/updateRefreshTokenByLogin")]
        public async Task<IActionResult> UpdateRefreshToken(string login, string token)
        {
            try
            {
                await _dynamoDbClient.ChangeTokenByLoginAsync(login, token);

                return StatusCode(200, "Updated");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPut]
        [Route("/updateLoginByLogin")]
        public async Task<IActionResult> UpdateLogin(string oldLogin, string newLogin)
        {
            try
            {
                await _dynamoDbClient.ChangeLoginByLoginAsync(oldLogin, newLogin);

                return StatusCode(200, "Updated");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
        
        [HttpDelete]
        [Route("/deleteRefreshTokenRecord")]
        public async Task<IActionResult> DeleteRecord(string login)
        {
            try
            {
                await _dynamoDbClient.DeleteRecordByLogin(login);

                return StatusCode(200, "Deleted");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
