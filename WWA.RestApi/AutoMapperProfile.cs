﻿using AutoMapper;
using WWA.GrainInterfaces.Models;
using WWA.RestApi.ViewModels.CurrentContext;
using WWA.RestApi.ViewModels.Games;
using WWA.RestApi.ViewModels.Users;

namespace WWA.RestApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // CurrentContext
            CreateMap<UserModel, CurrentContextReadViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            // Games
            CreateMap<GameCreateViewModel, GameModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<GameUpdateViewModel, GameModel>();
            CreateMap<GameModel, GameSummaryViewModel>();
            CreateMap<GameModel, GameReadViewModel>();
            // Users
            CreateMap<UserCreateViewModel, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UserReplaceViewModel, UserModel> ();
            CreateMap<UserModel, UserSummaryViewModel>();
            CreateMap<UserModel, UserReadViewModel>();
        }
    }
}
