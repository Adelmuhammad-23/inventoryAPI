using AutoMapper;

namespace IMS.Application.Mapping.UserMapping
{
    public partial class UserProfile : Profile
    {
        public UserProfile()
        {
            AddUserCommandMapping();
            UpdateUserMapping();
        }
    }
}
