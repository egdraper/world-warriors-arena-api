using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Constants;
using WWA.Grains.Maps.Entities;
using WWA.Grains.Mongo;

namespace WWA.Grains.Maps
{
    public interface IWorldMapRepository
    {
        Task<int> ExistsAsync(string gameId, string createdBy, string name);
        Task<WorldMap> GetWorldMapAsync(string id);
        Task<WorldMap> GetWorldMapAsync(string gameId, string id);
        Task<PaginatedEntityModel<WorldMap>> ListWorldMapsPagedAsync(string gameId, PaginationQueryModel paginationQuery);
        Task<WorldMap> CreateMapAsync(WorldMap worldMap);
        Task DeleteMapAsync(string id);
    }

    public class WorldMapRepository : Repository<WorldMap>, IWorldMapRepository
    {
        private ILogger<IWorldMapRepository> _logger;
        private IMapper _mapper;
        public WorldMapRepository(
            ILogger<WorldMapRepository> logger,
            IMapper mapper,
            IMongoContext mongoContext) : base(mongoContext, MongoCollections.WorldMaps)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public Task<int> ExistsAsync(string gameId, string createdBy, string name)
        {
            return QueryAsync(_filter.And(
                _filter.Eq("GameId", gameId),
                _filter.Eq("CreatedBy", createdBy),
                _filter.Eq("Name", name)));
        }

        public async Task<WorldMap> GetWorldMapAsync(string id)
        {
            return await GetAsync(_filter.Eq("_id", ObjectId.Parse(id)));
        }
        public async Task<WorldMap> GetWorldMapAsync(string gameId, string id)
        {
            return await GetAsync(_filter.And(
                _filter.Eq("GameId", gameId),
                _filter.Eq("_id", ObjectId.Parse(id))));
        }

        public Task<PaginatedEntityModel<WorldMap>> ListWorldMapsPagedAsync(string gameId, PaginationQueryModel paginationQuery)
        {
            var filter = string.IsNullOrEmpty(paginationQuery.Search)
                ? _filter.Eq("GameId", gameId)
                : _filter.And(_filter.Eq("GameId", gameId), _filter.Text(paginationQuery.Search));
            SortDefinition<WorldMap>? sort = null;
            if (!string.IsNullOrEmpty(paginationQuery.SortField))
            {
                if (paginationQuery.SortDirection == SortDirectionType.Ascending)
                {
                    sort = _sort.Ascending(paginationQuery.SortField);
                }
                else if (paginationQuery.SortDirection == SortDirectionType.Descending)
                {
                    sort = _sort.Descending(paginationQuery.SortField);
                }
            }
            var page = ListPagedAsync(
                filter,
                sort,
                paginationQuery.Skip,
                paginationQuery.Take);
            return page;
        }

        public async Task<WorldMap> CreateMapAsync(WorldMap map)
        {
            await CreateAsync(map);
            return map;
        }

        public async Task DeleteMapAsync(string id)
        {
            await DeleteAsync(id);
            return;
        }
    }
}
