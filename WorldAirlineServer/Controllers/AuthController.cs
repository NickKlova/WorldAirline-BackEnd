using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AWSDatabase.Administration.Managment;
using AWSDatabase.Extensions;
using AWSDatabase.Models.AmazonResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
