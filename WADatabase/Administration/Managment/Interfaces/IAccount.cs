using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment.Interfaces
{
    public interface IAccount
    {
        public Task<Models.API.Response.ReturnAccount> GetAccountAsync(int? id);
        public Task<Models.API.Response.ReturnAccount> GetAccountAsync(string login);
        public Task<IEnumerable<Models.API.Response.ReturnAccount>> GetAccountsByPersonalInfo(string name, string surname);
        public Task RegisterAccountAsync(Models.API.Request.ReceivedAccount model);
        public Task ChangeAccountLoginAsync(string oldLogin, string newLogin, string password);
        public Task ChangeAccountPasswordAsync(string login, string oldPassword, string newPassword);
        public Task ChangeAccountBalanceAsync(decimal amount, string login);
        public Task DeleteAccountAsync(string login);
        public Task<ClaimsIdentity> GetIdentity(string login, string password);
    }
}
