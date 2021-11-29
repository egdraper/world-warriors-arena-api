using AutoMapper;
using WWA.GrainInterfaces.Models;
using WWA.RestApi.ViewModels.Users;

namespace WWA.RestApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserCreateViewModel, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UserReplaceViewModel, UserModel> ();
            CreateMap<UserModel, UserSummaryViewModel>();
            CreateMap<UserModel, UserReadViewModel>();
        }
    }
}
