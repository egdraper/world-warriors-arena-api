using AutoMapper;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Games.Entities;

namespace WWA.Grains.Games
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GameModel, GameState>();
            CreateMap<GameModel, Game>();
            CreateMap<GameUpdateModel, GameState>();
            CreateMap<GameState, GameModel>();
            CreateMap<GameState, GameUpdateModel>();
            CreateMap<GameState, Game>();
            CreateMap<Game, GameModel>();
            CreateMap<Game, GameState>();
        }
    }
}
