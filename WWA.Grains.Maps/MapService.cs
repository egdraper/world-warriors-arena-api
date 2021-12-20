using AutoMapper;
using Microsoft.Extensions.Logging;
using Orleans;
using WWA.GrainInterfaces;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Maps;
using WWA.Grains.Maps.Entities;

namespace WWA.Grains.Maps
{
    public class MapService : Grain<Map>, IMapService
    {
        private readonly IClusterClient _clusterClient;
        private readonly ILogger<IMapService> _logger;
        private readonly IMapper _mapper;
        private readonly IMapRepository _mapRepository;

        public MapService(
            IClusterClient clusterClient,
            ILogger<MapService> logger,
            IMapper mapper,
            IMapRepository mapRepository)
        {
            _clusterClient = clusterClient;
            _logger = logger;
            _mapper = mapper;
            _mapRepository = mapRepository;
        }

        public async Task<int> QueryMapsAsync(string ownedBy, string name)
        {
            int count = await _mapRepository.ExistsAsync(ownedBy, name);
            return count;
        }

        public async Task<PaginatedEntityModel<MapModel>> GetMapsAsync(string userId, PaginationQueryModel paginationQuery)
        {
            PaginatedEntityModel<MapModel> mapModels = new PaginatedEntityModel<MapModel>();
            try
            {
                var page = await _mapRepository.ListMapsPagedAsync(userId, paginationQuery);
                mapModels.TotalCount = page.TotalCount;
                mapModels.Page = _mapper.Map<IList<MapModel>>(page.Page);
            }
            catch
            {
                throw;
            }
            return mapModels;
        }

        public async Task<MapModel> GetMapAsync(string userId, string id)
        {
            var mapGrain = _clusterClient.GetGrain<IMapGrain>(id);
            var map = await mapGrain.GetMapAsync();

            return _mapper.Map<MapModel>(map);
        }

        public async Task<MapModel> CreateMapAsync(string userId, MapModel mapModel)
        {
            mapModel.CreatedBy= userId;

            int count = await _mapRepository.ExistsAsync(mapModel.CreatedBy, mapModel.Name);
            if (count > 0)
            {
                throw new Exception($"Map named {mapModel.Name} already exists");
            }
            var map = _mapper.Map<Map>(mapModel);
            await _mapRepository.CreateMapAsync(map);

            var mapGrain = _clusterClient.GetGrain<IMapGrain>(map.Id);
            var newMapModel = await mapGrain.GetMapAsync();

            return _mapper.Map<MapModel>(newMapModel);
        }

        public async Task<MapModel> UpdateMapAsync(string userId, string id, MapUpdateModel mapUpdateModel)
        {
            var mapGrain = _clusterClient.GetGrain<IMapGrain>(id);
            var newMapState = await mapGrain.UpdateMapAsync(userId, mapUpdateModel);

            return _mapper.Map<MapModel>(newMapState);
        }

        public async Task DeleteMapAsync(string userId, string id)
        {
            var mapGrain = _clusterClient.GetGrain<IMapGrain>(id);
            await mapGrain.DeleteMapAsync();
            await _mapRepository.DeleteMapAsync(id);
            return;
        }
    }
}
