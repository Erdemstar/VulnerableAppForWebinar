using MongoDB.Bson.Serialization.Attributes;
using VulnerableAppForWebinar.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Entity.Profile
{
    public class ProfileEntity : BaseEntity
    {
        public string Email { get; set; }
        public string Hobby { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
    }
}
