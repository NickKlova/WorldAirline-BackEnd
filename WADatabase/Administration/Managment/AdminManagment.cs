using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Administration.Managment
{
    class AdminManagment
    {
        //public async Task GiveRole(string incRole, string login)
        //{
        //    WorldAirlinesClient db = new WorldAirlinesClient();

        //    await using (db.context)
        //    {
        //        var account = db.context.Accounts
        //            .ToListAsync()
        //            .Result
        //            .FirstOrDefault(x => x.Login == login);

        //        int? role = db.context.Roles
        //            .ToListAsync()
        //            .Result
        //            .FirstOrDefault(x => x.Role1 == incRole).Id;

        //        if (role == null)
        //            throw new Exception("Not found");
        //        else
        //            account.RoleId = role;
        //        db.context.SaveChanges();
        //    }
        //}


    }
}
