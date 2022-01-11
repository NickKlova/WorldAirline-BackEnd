using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AWSDatabase.Extensions;
using AWSDatabase.Models.AmazonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSDatabase.Administration.Managment
{
    public class AuthManagment 
    {
        private string _tableName;
        private readonly IAmazonDynamoDB _dynamoDb;
        
        public AuthManagment(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
            _tableName = "auth-db";
        }
        public async Task<RefreshToken> GetTokenByLoginAsync(string login)
        {
            var item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"login", new AttributeValue{S = login} }
                }
            };

            var response = await _dynamoDb.GetItemAsync(item);
            if (response.Item == null || !response.IsItemSet)
                return null;

            var result = response.Item.ToClass<RefreshToken>();
            
            return result;
        }

        public async Task CreateRecord(RefreshToken incomingData)
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"login", new AttributeValue{ S = incomingData.login } },
                    {"refreshToken", new AttributeValue{ S = incomingData.refreshToken } }
                }
            };

            await _dynamoDb.PutItemAsync(request);
        }

        public async Task ChangeTokenByLoginAsync(string login, string token)
        {
            await DeleteRecordByLogin(login);

            RefreshToken item = new RefreshToken
            {
                login = login,
                refreshToken = token
            };

            await CreateRecord(item);
        }

        public async Task ChangeLoginByLoginAsync(string oldLogin, string newLogin)
        {
            var record = await GetTokenByLoginAsync(oldLogin);

            await DeleteRecordByLogin(oldLogin);

            record.login = newLogin;

            await CreateRecord(record);
        }

        public async Task DeleteRecordByLogin(string login)
        {
            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"login", new AttributeValue{ S = login } }
                }
            };

            await _dynamoDb.DeleteItemAsync(request);
        }
    }
}
