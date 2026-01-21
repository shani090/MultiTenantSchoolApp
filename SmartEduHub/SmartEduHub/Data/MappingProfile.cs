using AutoMapper;
using SmartEduHub.DTO;
using SmartEduHub.DTO.CollageDTO;
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
            CreateMap<CollegeCreateDTO, College>()
           .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
           .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<CollegeUpdateDTO, College>();

            CreateMap<College, CollegeResponseDTO>();
        }
    }
    
}
