using Orleans;
using System.Threading.Tasks;
using WWA.GrainInterfaces.Models;

namespace WWA.GrainInterfaces
{
    public interface IGameService : IGrainWithGuidKey
    {
        Task<int> QueryGamesAsync(string ownedBy, string name);
        Task<PaginatedEntityModel<GameModel>> GetGamesAsync(string userId, PaginationQueryModel paginationQuery);
        Task<GameModel> GetGameAsync(string userId, string id);
        Task<GameModel> CreateGameAsync(string userId, GameModel gameModel);
        Task<GameModel> UpdateGameAsync(string userId, string id, GameUpdateModel gameUpdateModel);
        Task DeleteGameAsync(string userId, string id);
    }
}
