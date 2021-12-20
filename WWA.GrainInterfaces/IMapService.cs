using Orleans;
using System.Threading.Tasks;
using WWA.GrainInterfaces.Models;

namespace WWA.GrainInterfaces
{
    public interface IMapService : IGrainWithGuidKey
    {
        public Task<int> QueryMapsAsync(string ownedBy, string name);
        public Task<PaginatedEntityModel<MapModel>> GetMapsAsync(string userId, PaginationQueryModel paginationQuery);
        public Task<MapModel> GetMapAsync(string userId, string id);
        public Task<MapModel> CreateMapAsync(string userId, MapModel mapModel);
        public Task<MapModel> UpdateMapAsync(string userId, string id, MapUpdateModel mapUpdateModel);
        public Task DeleteMapAsync(string userId, string id);
    }
}
