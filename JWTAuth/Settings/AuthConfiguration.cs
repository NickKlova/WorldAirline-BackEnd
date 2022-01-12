using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Settings
{
    public class AuthConfiguration
    {
        public static string ISSUER = "WorldAirlinesAuthServer"; 
        public static string AUDIENCE = "WorldAirlinesClient"; 
        public static string KEY { get; set; }
        public static int LIFETIME = 600; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
