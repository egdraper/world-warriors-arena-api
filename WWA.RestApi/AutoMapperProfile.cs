using AutoMapper;
using WWA.GrainInterfaces.Models;
using WWA.RestApi.ViewModels.CurrentContext;
using WWA.RestApi.ViewModels.Games;
using WWA.RestApi.ViewModels.Maps;
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
            CreateMap<GameModel, GameSummaryViewModel>();
            CreateMap<GameModel, GameReadViewModel>();
            CreateMap<GameModel, GameUpdateViewModel>();
            CreateMap<GameUpdateViewModel, GameUpdateModel>();
            // Maps
            CreateMap<WorldMapCreateViewModel, WorldMapModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<WorldMapModel, WorldMapSummaryViewModel>();
            CreateMap<WorldMapModel, WorldMapReadViewModel>();
            CreateMap<WorldMapModel, WorldMapUpdateViewModel>();
            CreateMap<WorldMapUpdateViewModel, WorldMapUpdateModel>();
            CreateMap<MapSizeModel, MapSizeViewModel>();
            CreateMap<MapSizeViewModel, MapSizeModel>();
            CreateMap<MapElevationModel, ElevationViewModel>();
            CreateMap<ElevationViewModel, MapElevationModel>();
                // Map Layers
                CreateMap<MapSpriteLayerModel, SpriteLayerViewModel>();
                CreateMap<MapTerrainSpriteLayerModel, TerrainSpriteLayerViewModel>();
                CreateMap<MapObjectSpriteLayerModel, ObjectSpriteLayerViewModel>();
                CreateMap<MapGatewayLayerModel, GatewayLayerViewModel>();
                CreateMap<SpriteLayerViewModel, MapSpriteLayerModel>();
                CreateMap<TerrainSpriteLayerViewModel, MapTerrainSpriteLayerModel>();
                CreateMap<ObjectSpriteLayerViewModel, MapObjectSpriteLayerModel>();
                CreateMap<GatewayLayerViewModel, MapGatewayLayerModel>();
                // Map Cells
                CreateMap<MapCellModel, CellViewModel>();
                CreateMap<CellViewModel, MapCellModel>();
                CreateMap<MapObjectCellModel, ObjectCellViewModel>();
                CreateMap<ObjectCellViewModel, MapObjectCellModel>();
                CreateMap<MapTerrainCellModel, TerrainCellViewModel>();
                CreateMap<TerrainCellViewModel, MapTerrainCellModel>();
                CreateMap<MapGatewayModel, GatewayViewModel>();
                CreateMap<GatewayViewModel, MapGatewayModel>();
                CreateMap<MapCoordinateModel, CoordinateViewModel>();
                CreateMap<CoordinateViewModel, MapCoordinateModel>();
            // Users
            CreateMap<UserCreateViewModel, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UserReplaceViewModel, UserModel> ();
            CreateMap<UserModel, UserSummaryViewModel>();
            CreateMap<UserModel, UserReadViewModel>();
        }
    }
}
