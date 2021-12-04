using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Utility.JWT
{
    public class JWTSettings : IJWTSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
}
