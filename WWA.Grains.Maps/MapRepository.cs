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
    public interface IMapRepository
    {
        Task<int> ExistsAsync(string ownedBy, string name);
        Task<Map> GetMapAsync(string id);
        Task<PaginatedEntityModel<Map>> ListMapsPagedAsync(string userId, PaginationQueryModel paginationQuery);
        Task<Map> CreateMapAsync(Map game);
        Task DeleteMapAsync(string id);
    }

    public class MapRepository : Repository<Map>, IMapRepository
    {
        private ILogger<IMapRepository> _logger;
        private IMapper _mapper;
        public MapRepository(
            ILogger<MapRepository> logger,
            IMapper mapper,
            IMongoContext mongoContext) : base(mongoContext, MongoCollections.Maps)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public Task<int> ExistsAsync(string ownedBy, string name)
        {
            return QueryAsync(_filter.And(_filter.Eq("OwnedBy", ownedBy), _filter.Eq("Name", name)));
        }

        public async Task<Map> GetMapAsync(string id)
        {
            return await GetAsync(_filter.Eq("_id", ObjectId.Parse(id)));
        }

        public Task<PaginatedEntityModel<Map>> ListMapsPagedAsync(string userId, PaginationQueryModel paginationQuery)
        {
            var filter = string.IsNullOrEmpty(paginationQuery.Search)
                ? _filter.AnyEq("Players", userId)
                : _filter.And(_filter.In("Players", userId), _filter.Text(paginationQuery.Search));
            SortDefinition<Map> sort = null;
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

        public async Task<Map> CreateMapAsync(Map map)
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
