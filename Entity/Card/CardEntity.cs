using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VulnerableAppForWebinar.Entity.Base;

namespace VulnerableAppForWebinar.Entity.Card
{
    public class CardEntity : BaseEntity
    {
        public string UserID { get; set; }
        public string Nickname { get; set; }
        public string Number { get; set; }
        public string ExpireDate { get; set; }
        public string Cve { get; set; }
        public string Password { get; set; }
    }
}
