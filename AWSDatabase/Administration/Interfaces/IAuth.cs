using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSDatabase.Administration.Interfaces
{
    public interface IAuth
    {
        public Task<Models.AmazonResponse.RefreshToken> GetTokenByLoginAsync(string login);
        public Task CreateRecord(Models.AmazonResponse.RefreshToken incomingData);
        public Task ChangeTokenByLoginAsync(string login, string token);
        public Task ChangeLoginByLoginAsync(string oldLogin, string newLogin);
        public Task DeleteRecordByLogin(string login);
    }
}
