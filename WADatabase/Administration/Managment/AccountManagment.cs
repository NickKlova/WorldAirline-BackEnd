using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;
using WADatabase.Models.API.Request;
using WADatabase.Models.API.Response;

namespace WADatabase.Administration.Managment
{
    public class AccountManagment : Interfaces.IAccount
    {
        private readonly WorldAirlinesClient _db;
        public AccountManagment(WorldAirlinesClient dbClient)
        {
            _db = dbClient;
        }
        public async Task<ReturnAccount> GetAccountAsync(int? id)
        {
            if (id == null)
                return null;

            ReturnAccount response;

            await using (_db)
            {
                var account = _db.context.Accounts
                    .Include(x=>x.Role)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (account == null)
                    return null;

                ReturnRole role;

                if (account.RoleId != null)
                {
                    role = new ReturnRole
                    {
                        Id = account.Role.Id,
                        Role = account.Role.Role1
                    };
                }
                else
                {
                    role = null;
                }

                response = new ReturnAccount
                {
                    Id = account.Id,
                    Name = account.Name,
                    Surname = account.Surname,
                    Email = account.Email,
                    Phone = account.Phone,
                    Balance = account.Balance,
                    Login = account.Login,
                    Role = role
                };
            }

            return response;
        }
        public async Task<ReturnAccount> GetAccountAsync(string login)
        {
            ReturnAccount response;

            await using (_db)
            {
                var account = _db.context.Accounts
                    .Include(x=>x.Role)
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == login);

                if (account == null)
                    return null;

                ReturnRole role;

                if (account.RoleId != null)
                {
                    role = new ReturnRole
                    {
                        Id = account.Role.Id,
                        Role = account.Role.Role1
                    };
                }
                else
                {
                    role = null;
                }

                response = new ReturnAccount
                {
                    Id = account.Id,
                    Name = account.Name,
                    Surname = account.Surname,
                    Email = account.Email,
                    Phone = account.Phone,
                    Balance = account.Balance,
                    Login = account.Login,
                    Role = role
                };
            }

            return response;
        }
        public async Task<IEnumerable<ReturnAccount>> GetAccountsByPersonalInfo(string name, string surname)
        {
            var response = new List<ReturnAccount>();

            await using (_db)
            {
                var accounts = _db.context.Accounts
                    .Include(x => x.Role)
                    .ToListAsync()
                    .Result
                    .Where(x => x.Name == name && x.Surname == surname);

                foreach(var account in accounts)
                {
                    ReturnRole role; 

                    if (account.RoleId != null)
                    {
                        role = new ReturnRole
                        {
                            Id = account.Role.Id,
                            Role = account.Role.Role1
                        };
                    }
                    else
                    {
                        role = null;
                    }

                    ReturnAccount item = new ReturnAccount
                    {
                        Id = account.Id,
                        Name = account.Name,
                        Surname = account.Surname,
                        Email = account.Email,
                        Phone = account.Phone,
                        Balance = account.Balance,
                        Login = account.Login,
                        Role = role
                    };

                    response.Add(item);
                }
            }

            return response;
        }
        public async Task RegisterAccountAsync(ReceivedAccount model)
        {
            if (!Settings.Validation.IsMailValid(model.Email))
                throw new Exception("Wrong mail!");

            if (Settings.Validation.PhoneValidation(model.Phone) == null)
                throw new Exception("Wrong phone number!");

            await using (_db)
            {
                var role = _db.context.Roles
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Role1 == "user");

                Models.DB_Request.Account account = new Models.DB_Request.Account
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Phone = Settings.Validation.PhoneValidation(model.Phone),
                    Balance = 0,
                    Login = model.Login,
                    Password = model.Password,
                    RoleId = role.Id
                };

                _db.context.Add(account);
                _db.context.SaveChanges();
            }
        }
        public async Task ChangeAccountLoginAsync(string oldLogin, string newLogin, string password)
        {
            await using (_db)
            {
                var account = _db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == oldLogin && x.Password == password);
                if (account == null)
                    throw new Exception("Bad data!");
                else
                {
                    account.Login = newLogin;
                    var result = _db.context.SaveChanges();
                }
            }
        }
        public async Task ChangeAccountPasswordAsync(string login, string oldPassword, string newPassword)
        {
            await using (_db)
            {
                var account = _db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == login && x.Password == oldPassword);

                if (account == null)
                    throw new Exception("Bad data!");
                else
                {
                    account.Password = newPassword;
                    _db.context.SaveChanges();
                }
            }
        }
        public async Task ChangeAccountBalanceAsync(decimal amount, string login)
        {
            await using (_db)
            {
                var account = _db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == login);

                if (account == null)
                    throw new Exception("Bad data!");
                else
                {
                    account.Balance += amount;
                    _db.context.SaveChanges();
                }
            }
        }
        public async Task DeleteAccountAsync(string login)
        {
            await using (_db)
            {
                var account = _db.context.Accounts
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Login == login);

                if (account == null)
                    throw new Exception("Bad data!");
                else
                {
                    _db.context.Remove(account);
                    _db.context.SaveChanges();
                }
            }
        }
        public async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            await using (_db)
            {
                var account = _db.context.Accounts
                   .Include(x=>x.Role)
                   .ToListAsync()
                   .Result
                   .FirstOrDefault(x => x.Login == login && x.Password == password);

                if (account != null)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, account.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role.Role1)
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
