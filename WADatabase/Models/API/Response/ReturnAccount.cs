using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Models.API.Response
{
    public class ReturnAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal? Balance { get; set; }
        public string Login { get; set; }
        public ReturnRole Role { get; set; }
    }
}
