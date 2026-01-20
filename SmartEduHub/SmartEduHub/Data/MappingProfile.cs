using AutoMapper;
using SmartEduHub.DTO;
using SmartEduHub.Models;

namespace SmartEduHub.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDTO, User>()
               .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())

               .ForMember(dest => dest.Id, opt => opt.Ignore())

               .ForMember(dest => dest.CollegeId, opt => opt.Ignore());
        }
    }
    
}
