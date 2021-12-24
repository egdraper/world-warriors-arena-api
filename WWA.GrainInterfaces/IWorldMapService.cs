using Orleans;
using System.Threading.Tasks;
using WWA.GrainInterfaces.Models;

namespace WWA.GrainInterfaces
{
    public interface IWorldMapService : IGrainWithStringKey
    {
        public Task<int> QueryMapsAsync(string ownedBy, string name);
        public Task<PaginatedEntityModel<WorldMapModel>> GetMapsAsync(string userId, PaginationQueryModel paginationQuery);
        public Task<WorldMapModel> GetMapAsync(string userId, string id);
        public Task<WorldMapModel> CreateMapAsync(string userId, WorldMapModel mapModel);
        public Task<WorldMapModel> UpdateMapAsync(string userId, string id, WorldMapUpdateModel mapUpdateModel);
        public Task DeleteMapAsync(string userId, string id);
    }
}
