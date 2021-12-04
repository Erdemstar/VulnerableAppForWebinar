using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Utility.JWT
{
    public interface IJWTSettings
    {
        string Key { get; set; }
        string Issuer { get; set; }
    }
}
