using VulnerableAppForWebinar.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Dto.Account
{
    public class AccountResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
