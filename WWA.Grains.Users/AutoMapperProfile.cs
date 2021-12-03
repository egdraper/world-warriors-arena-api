using AutoMapper;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Users.Entities;

namespace WWA.Grains.Users
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, UserState>();
            CreateMap<UserModel, User>();
            CreateMap<UserState, UserModel>();
            CreateMap<UserState, User>();
            CreateMap<User, UserModel>();
            CreateMap<User, UserState>();
        }
    }
}
