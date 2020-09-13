using API.Test.Domain;
using AutoMapper;
using DTO = API.Test.Infrastructure.DTOs;

namespace API.Test.Infrastructure.Concrete.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Permission, DTO.PermissionResponse>();
            CreateMap<DTO.PermissionRequest, Permission>()
               .ForMember(x => x.Id, opts => opts.Ignore());
            CreateMap<PermissionType, DTO.PermissionType>();
        }
    }
}