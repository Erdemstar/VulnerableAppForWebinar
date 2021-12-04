using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Dto.Others
{
    public class BaseResponse : IBaseResponse
    {
        public string Message { get; set; }
    }
}
