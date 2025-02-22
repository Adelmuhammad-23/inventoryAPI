using IMS.Application.DTOs.AccountDTOs;
using IMS.Domain.Entities.Identities;

namespace IMS.Application.Mapping.UserMapping
{
    public partial class UserProfile
    {
        public void AddUserCommandMapping()
        {
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(p => p.Phone));
        }
    }
}
