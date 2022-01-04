using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;

namespace WADatabase.Administration.Managment
{
    public class AccountManagment
    {
        public async Task RegistrationAsync(Models.API.Request.ReceivedAccount incomingData)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            Models.DB_Request.Account account = new Models.DB_Request.Account
            {
                Name = incomingData.Name,
                Surname = incomingData.Surname,
                Email = incomingData.Email,
                Phone = incomingData.Phone,
                Balance = 0,
                Login = incomingData.Login,
                Password = incomingData.Password,
                RoleId = 1
            };

            await using (db.context)
            {
                var result = db.context.Add(account);

                if (result.State != EntityState.Added)
                    throw new Exception("Bad request");

                db.context.SaveChanges();
            }
        }

        public async Task ChangeBalanceAsync(decimal amount, string login)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var account = db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == login);

                account.Balance += amount;

                db.context.SaveChanges();
            }
        }

        public async Task<Models.API.Response.ReturnAccount> GetByIdAsync(int id)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            Models.API.Response.ReturnAccount response;

            await using (db.context)
            {
                var account = db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                response = new Models.API.Response.ReturnAccount
                {
                    Id = account.Id,
                    Name = account.Name,
                    Surname = account.Surname,
                    Email = account.Email,
                    Phone = account.Phone,
                    Balance = account.Balance,
                    Login = account.Login,
                    Role = new Models.API.Response.ReturnRole
                    {
                        Id = account.Role.Id,
                        Role = account.Role.Role1
                    }
                };
            }

            return response;
        }

        public async Task<Models.API.Response.ReturnAccount> GetByLoginAsync(string login)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            Models.API.Response.ReturnAccount response;

            await using (db.context)
            {
                var account = db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == login);

                response = new Models.API.Response.ReturnAccount
                {
                    Id = account.Id,
                    Name = account.Name,
                    Surname = account.Surname,
                    Email = account.Email,
                    Phone = account.Phone,
                    Balance = account.Balance,
                    Login = account.Login,
                    Role = new Models.API.Response.ReturnRole
                    {
                        Id = account.Role.Id,
                        Role = account.Role.Role1
                    }
                };
            }

            return response;
        }

        public async Task ChangeLoginAsync(string login)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var account = db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == login);

                account.Login = login;
                db.context.SaveChanges();
            }
        }

        public async Task ChangePasswordAsync(string login, string password)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var account = db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == login);

                account.Password = password;
                db.context.SaveChanges();
            }
        }

        public async Task DeleteByLoginAsync(string login)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var account = db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == login);

                var result = db.context.Remove(account);
                db.context.SaveChanges();

                if (result.State != EntityState.Deleted)
                    throw new Exception("Bad request");
            }
        }
    }
}
