using AutoMapper;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Maps.Entities;

namespace WWA.Grains.Maps
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Root
            CreateMap<WorldMapModel, WorldMapState>();
            CreateMap<WorldMapModel, WorldMap>();
            CreateMap<WorldMapUpdateModel, WorldMapState>();
            CreateMap<WorldMapState, WorldMapModel>();
            CreateMap<WorldMapState, WorldMapUpdateModel>();
            CreateMap<WorldMapState, WorldMap>();
            CreateMap<WorldMap, WorldMapModel>();
            CreateMap<WorldMap, WorldMapState>();
            CreateMap<MapSizeModel, MapSize>();
            CreateMap<MapSize, MapSizeModel>();
            CreateMap<MapElevationModel, MapElevation>();
            CreateMap<MapElevation, MapElevationModel>();
            // Layers
            CreateMap<MapSpriteLayerModel, SpriteLayer>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());
            CreateMap<MapTerrainSpriteLayerModel, TerrainSpriteLayer>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());
            CreateMap<MapObjectSpriteLayerModel, ObjectSpriteLayer>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());
            CreateMap<MapGatewayLayerModel, GatewayLayer>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());
            CreateMap<SpriteLayer, MapSpriteLayerModel>();
            CreateMap<TerrainSpriteLayer, MapTerrainSpriteLayerModel>();
            CreateMap<ObjectSpriteLayer, MapObjectSpriteLayerModel>();
            CreateMap<GatewayLayer, MapGatewayLayerModel>();
            // Cells
            CreateMap<MapCellModel, Cell>();
            CreateMap<Cell, MapCellModel>();
            CreateMap<MapObjectCellModel, ObjectCell>();
            CreateMap<ObjectCell, MapObjectCellModel>();
            CreateMap<MapTerrainCellModel, TerrainCell>();
            CreateMap<TerrainCell, MapTerrainCellModel>();
            CreateMap<MapGatewayModel, Gateway>();
            CreateMap<Gateway, MapGatewayModel>();
            CreateMap<MapCoordinateModel, MapCoordinate>();
            CreateMap<MapCoordinate, MapCoordinateModel>();
        }
    }
}
