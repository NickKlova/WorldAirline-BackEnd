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
        public static string AUDIENCE = "WorldAirlinesFrontEndClient"; 
        public static string KEY = "V29ybGRBaXJsaW5lc0NvbXBhbnlBbGxSaWdodHNBcmVSZXNlcnZlZA";  
        public static int LIFETIME = 600; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
