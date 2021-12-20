using AutoMapper;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Maps.Entities;

namespace WWA.Grains.Maps
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MapModel, MapState>();
            CreateMap<MapModel, Map>();
            CreateMap<MapUpdateModel, MapState>();
            CreateMap<MapState, MapModel>();
            CreateMap<MapState, MapUpdateModel>();
            CreateMap<MapState, Map>();
            CreateMap<Map, MapModel>();
            CreateMap<Map, MapState>();
        }
    }
}
