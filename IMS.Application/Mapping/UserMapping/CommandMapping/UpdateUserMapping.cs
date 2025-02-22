using IMS.Application.DTOs.AccountDTOs;
using IMS.Domain.Entities.Identities;

namespace IMS.Application.Mapping.UserMapping
{
    public partial class UserProfile
    {
        public void UpdateUserMapping()
        {
            CreateMap<UpdateProfileDTO, User>();
        }
    }
}
