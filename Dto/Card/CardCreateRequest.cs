using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Dto.Card
{
    public class CardCreateRequest
    {
        public string Nickname { get; set; }
        public string Number { get; set; }
        public string ExpireDate { get; set; }
        public string Cve { get; set; }
        public string Password { get; set; }
    }
}
