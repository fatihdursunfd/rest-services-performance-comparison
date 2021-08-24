using AutoMapper;

namespace WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User , UserGetView>();

            CreateMap<UserPostView , User>();

            CreateMap<UserPutView , User>();

        }
    }
}