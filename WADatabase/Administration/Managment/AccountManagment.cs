using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                RoleId = 3
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

                var role = db.context.Roles
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == Convert.ToInt32(account.RoleId));

                if (role != null)
                {
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
                            Id = role.Id,
                            Role = role.Role1
                        }
                    };
                }
                else
                {
                    response = new Models.API.Response.ReturnAccount
                    {
                        Id = account.Id,
                        Name = account.Name,
                        Surname = account.Surname,
                        Email = account.Email,
                        Phone = account.Phone,
                        Balance = account.Balance,
                        Login = account.Login,
                        Role = null
                    };
                }
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

                var role = db.context.Roles
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == Convert.ToInt32(account.RoleId));

                if (role != null)
                {
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
                            Id = role.Id,
                            Role = role.Role1
                        }
                    };
                }
                else
                {
                    response = new Models.API.Response.ReturnAccount
                    {
                        Id = account.Id,
                        Name = account.Name,
                        Surname = account.Surname,
                        Email = account.Email,
                        Phone = account.Phone,
                        Balance = account.Balance,
                        Login = account.Login,
                        Role = null
                    };
                }
            }

            return response;
        }

        public async Task ChangeLoginAsync(string oldLogin, string newLogin)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var account = db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == oldLogin);

                account.Login = newLogin;
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

                if (result.State != EntityState.Detached)
                    throw new Exception("Bad request");
            }
        }

        public async Task GiveRole(string incRole, string login)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var account = db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == login);

                int? role = db.context.Roles
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Role1 == incRole).Id;

                if (role == null)
                    throw new Exception("Not found");
                else
                    account.RoleId = role;
                db.context.SaveChanges();
            }
        }

        public async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var account = db.context.Accounts
                   .ToListAsync()
                   .Result
                   .FirstOrDefault(x => x.Login == login && x.Password == password);

                var role = db.context.Roles
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == Convert.ToInt32(account.RoleId));

                if (account != null)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, account.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Role1)
                };
                    ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }
            }
            return null;
        }
    }
}
