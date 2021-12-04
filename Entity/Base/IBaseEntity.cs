using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Entity.Base
{
    public interface IBaseEntity
    {
        string Id { get; }
        DateTime CreatedAt { get; set; }
    }
}
