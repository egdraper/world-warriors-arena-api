using Orleans;
using System.Threading.Tasks;
using WWA.GrainInterfaces.Models;

namespace WWA.GrainInterfaces
{
    public interface IUserService : IGrainWithGuidKey
    {
        public Task<UserModel> AuthenticateUserAsync(string email, string password);
        public Task<int> QueryUsersAsync(string email);
        public Task<PaginatedEntityModel<UserModel>> GetUsersAsync(PaginationQueryModel paginationQueryModel);
        public Task<UserModel> GetUserAsync(string id);
        public Task<UserModel> CreateUserAsync(UserModel userModel);
        public Task<UserModel> ReplaceUserAsync(UserModel userModel);
        public Task DeleteUserAsync(string id);
    }
}