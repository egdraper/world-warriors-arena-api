using AutoMapper;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Constants;
using WWA.Grains.Maps.Entities;

namespace WWA.Grains.Maps
{
    public interface IMapGrain : IGrainWithStringKey
    {
        Task<MapState> GetMapAsync();
        Task<MapState> UpdateMapAsync(string userId, MapUpdateModel mapUpdateModel);
        Task DeleteMapAsync();
    }

    public class MapGrain : Grain<MapState>, IMapGrain
    {
        private readonly IPersistentState<MapState> _map;
        private readonly ILogger<IMapGrain> _logger;
        private readonly IMapper _mapper;
        private readonly IMapRepository _mapRepository;

        public MapGrain(
            [PersistentState(MongoCollections.MapState, MongoStorageProviders.GrainState)]
            IPersistentState<MapState> map,
            ILogger<MapGrain> logger,
            IMapper mapper,
            IMapRepository mapRepository)
        {
            _map = map;
            _logger = logger;
            _mapper = mapper;
            _mapRepository = mapRepository;
        }

        public override async Task OnActivateAsync()
        {
            var primaryKey = this.GetPrimaryKeyString();
            if (_map.State.Id == null)
            {
                Map map = await _mapRepository.GetMapAsync(primaryKey);
                _mapper.Map(map, _map.State);
                _map.State.Id = primaryKey;
            }
            await _map.WriteStateAsync();
            await base.OnActivateAsync();
            return;
        }

        public Task<MapState> GetMapAsync()
        {
            if (_map.State == null)
            {
                throw new Exception($"Unable to load Map with id: {this.GetPrimaryKeyString()}");
            }
            return Task.FromResult(_map.State);
        }

        public Task<MapState> UpdateMapAsync(string userId, MapUpdateModel mapUpdateModel)
        {
            if (userId != _map.State.CreatedBy)
            {
                throw new Exception($"User '{userId}' does not have access to modify this map");
            }
            _mapper.Map(mapUpdateModel, _map.State);
            _map.State.DateModified = DateTime.UtcNow;
            _map.WriteStateAsync();

            return Task.FromResult(_map.State);
        }

        public Task DeleteMapAsync()
        {
            _map.ClearStateAsync();
            this.DeactivateOnIdle();
            return Task.CompletedTask;
        }
    }
}
