using AutoMapper;
using VulnerableAppForWebinar.Dto.Account;
using VulnerableAppForWebinar.Entity.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // source -> dest
            CreateMap<RegisterRequest, AccountEntity>();
            CreateMap<AccountEntity,RegisterRequest>();
            CreateMap<LoginRequest, AccountEntity>();
            CreateMap<AccountEntity,LoginRequest>();
            CreateMap<AccountEntity, AccountResponse>();
        }
    }
}
