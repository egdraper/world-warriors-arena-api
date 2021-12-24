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
        Task<WorldMapState> GetWorldMapAsync();
        Task<WorldMapState> UpdateWorldMapAsync(string userId, WorldMapUpdateModel mapUpdateModel);
        Task DeleteWorldMapAsync();
    }

    public class WorldMapGrain : Grain<WorldMapState>, IMapGrain
    {
        private readonly IPersistentState<WorldMapState> _worldMap;
        private readonly ILogger<IMapGrain> _logger;
        private readonly IMapper _mapper;
        private readonly IWorldMapRepository _worldMapRepository;

        public WorldMapGrain(
            [PersistentState(MongoCollections.WorldMapState, MongoStorageProviders.GrainState)]
            IPersistentState<WorldMapState> worldMap,
            ILogger<WorldMapGrain> logger,
            IMapper mapper,
            IWorldMapRepository worldMapRepository)
        {
            _worldMap = worldMap;
            _logger = logger;
            _mapper = mapper;
            _worldMapRepository = worldMapRepository;
        }

        public override async Task OnActivateAsync()
        {
            var primaryKey = this.GetPrimaryKeyString();
            if (_worldMap.State.Id == null)
            {
                WorldMap map = await _worldMapRepository.GetWorldMapAsync(primaryKey);
                _mapper.Map(map, _worldMap.State);
                _worldMap.State.Id = primaryKey;
                _worldMap.State.Elevations = new()
                {
                    ["0"] = new()
                    {
                        BaseLayer = new(),
                        TerrainLayer = new(),
                        StructureLayer = new(),
                        PartitionLayer = new(),
                        CeilingObjectLayer = new(),
                        FloorObjectLayer = new(),
                        SuspendedObjectLayer = new(),
                        WallObjectLayer = new(),
                        GatewayLayer = new()
                    }
                };
            }
            await _worldMap.WriteStateAsync();
            await base.OnActivateAsync();
            return;
        }

        public Task<WorldMapState> GetWorldMapAsync()
        {
            if (_worldMap.State == null)
            {
                throw new Exception($"Unable to load Map with id: {this.GetPrimaryKeyString()}");
            }
            return Task.FromResult(_worldMap.State);
        }

        public Task<WorldMapState> UpdateWorldMapAsync(string userId, WorldMapUpdateModel mapUpdateModel)
        {
            if (userId != _worldMap.State.CreatedBy)
            {
                throw new Exception($"User '{userId}' does not have access to modify this map");
            }
            _mapper.Map(mapUpdateModel, _worldMap.State);
            _worldMap.State.DateModified = DateTime.UtcNow;
            _worldMap.WriteStateAsync();

            return Task.FromResult(_worldMap.State);
        }

        public Task DeleteWorldMapAsync()
        {
            _worldMap.ClearStateAsync();
            this.DeactivateOnIdle();
            return Task.CompletedTask;
        }
    }
}
