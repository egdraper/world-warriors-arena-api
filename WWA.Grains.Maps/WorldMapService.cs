using AutoMapper;
using Microsoft.Extensions.Logging;
using Orleans;
using WWA.GrainInterfaces;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Maps.Entities;

namespace WWA.Grains.Maps
{
    public class WorldMapService : Grain<WorldMap>, IWorldMapService
    {
        private readonly IClusterClient _clusterClient;
        private readonly ILogger<IWorldMapService> _logger;
        private readonly IMapper _mapper;
        private readonly IWorldMapRepository _worldMapRepository;

        public WorldMapService(
            IClusterClient clusterClient,
            ILogger<WorldMapService> logger,
            IMapper mapper,
            IWorldMapRepository worldMapRepository)
        {
            _clusterClient = clusterClient;
            _logger = logger;
            _mapper = mapper;
            _worldMapRepository = worldMapRepository;
        }

        public async Task<int> QueryMapsAsync(string createdBy, string name)
        {
            int count = await _worldMapRepository.ExistsAsync(this.GetPrimaryKeyString(), createdBy, name);
            return count;
        }

        public async Task<PaginatedEntityModel<WorldMapModel>> GetMapsAsync(string userId, PaginationQueryModel paginationQuery)
        {
            PaginatedEntityModel<WorldMapModel> mapModels = new PaginatedEntityModel<WorldMapModel>();
            try
            {
                var page = await _worldMapRepository.ListWorldMapsPagedAsync(this.GetPrimaryKeyString(), paginationQuery);
                mapModels.TotalCount = page.TotalCount;
                mapModels.Page = _mapper.Map<IList<WorldMapModel>>(page.Page);
            }
            catch
            {
                throw;
            }
            return mapModels;
        }

        public async Task<WorldMapModel> GetMapAsync(string userId, string id)
        {
            WorldMapState map;
            try
            {
                var gameService = _clusterClient.GetGrain<IGameService>(Guid.Empty);
                int gameExists = await gameService.QueryGamesAsync(gameId: this.GetPrimaryKeyString(), playerId: userId);
                if (gameExists < 1)
                {
                    throw new Exception("Game does not exist or user is not a player");
                }
                var mapGrain = _clusterClient.GetGrain<IMapGrain>(id);
                map = await mapGrain.GetWorldMapAsync();
            }
            catch
            {
                throw;
            }

            return _mapper.Map<WorldMapModel>(map);
        }

        public async Task<WorldMapModel> CreateMapAsync(string userId, WorldMapModel mapModel)
        {
            mapModel.GameId = this.GetPrimaryKeyString();
            mapModel.CreatedBy = userId;
            mapModel.Size = new()
            {
                Width = 64,
                Height = 64
            };

            var gameService = _clusterClient.GetGrain<IGameService>(Guid.Empty);
            int gameExists = await gameService.QueryGamesAsync(gameId: this.GetPrimaryKeyString(), ownedBy: userId);
            if (gameExists < 1)
            {
                throw new Exception("Game does not exist or user is not the owner");
            }

            int count = await _worldMapRepository.ExistsAsync(
                            this.GetPrimaryKeyString(),
                            mapModel.CreatedBy,
                            mapModel.Name);
            if (count > 0)
            {
                throw new Exception($"Map named {mapModel.Name} already exists");
            }
            var map = _mapper.Map<WorldMap>(mapModel);
            await _worldMapRepository.CreateMapAsync(map);

            var mapGrain = _clusterClient.GetGrain<IMapGrain>(map.Id);
            var newMapModel = await mapGrain.GetWorldMapAsync();

            return _mapper.Map<WorldMapModel>(newMapModel);
        }

        public async Task<WorldMapModel> UpdateMapAsync(string userId, string id, WorldMapUpdateModel mapUpdateModel)
        {
            var gameService = _clusterClient.GetGrain<IGameService>(Guid.Empty);
            int gameExists = await gameService.QueryGamesAsync(gameId: this.GetPrimaryKeyString(), ownedBy: userId);
            if (gameExists < 1)
            {
                throw new Exception("Game does not exist or user is not the owner");
            }

            var mapGrain = _clusterClient.GetGrain<IMapGrain>(id);
            var newMapState = await mapGrain.UpdateWorldMapAsync(userId, mapUpdateModel);

            return _mapper.Map<WorldMapModel>(newMapState);
        }

        public async Task DeleteMapAsync(string userId, string id)
        {
            var gameService = _clusterClient.GetGrain<IGameService>(Guid.Empty);
            int gameExists = await gameService.QueryGamesAsync(gameId: this.GetPrimaryKeyString(), ownedBy: userId);
            if (gameExists < 1)
            {
                throw new Exception("Game does not exist or user is not the owner");
            }

            var mapGrain = _clusterClient.GetGrain<IMapGrain>(id);
            await mapGrain.DeleteWorldMapAsync();
            await _worldMapRepository.DeleteMapAsync(id);
            return;
        }
    }
}
