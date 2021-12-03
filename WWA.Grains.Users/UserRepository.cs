using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Constants;
using WWA.Grains.Mongo;
using WWA.Grains.Users.Entities;

namespace WWA.Grains.Users
{
    public interface IUserRepository
    {
        Task<User> AuthenticateUserAsync(string email, string password);
        Task<int> ExistsAsync(string email);
        Task<User> GetAsync(string id);
        Task<PaginatedEntityModel<User>> ListUsersPagedAsync(PaginationQueryModel paginationQuery);
        Task<User> CreateUserAsync(User user);
        Task DeleteUserAsync(string id);
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        private ILogger<IUserRepository> _logger;
        private IMapper _mapper;
        public UserRepository(
            ILogger<UserRepository> logger,
            IMapper mapper,
            IMongoContext mongoContext) : base(mongoContext, MongoCollections.Users)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            return await GetAsync(_filter.And(_filter.Eq("Email", email), _filter.Eq("Password", password)));
        }

        public Task<int> ExistsAsync(string email)
        {
            return QueryAsync(_filter.Eq("Email", email));
        }

        public async Task<User> GetAsync(string id)
        {
            return await GetAsync(_filter.Eq("_id", ObjectId.Parse(id)));
        }

        public Task<PaginatedEntityModel<User>> ListUsersPagedAsync(PaginationQueryModel paginationQuery)
        {
            var filter = string.IsNullOrEmpty(paginationQuery.Search)
                ? _filter.Empty
                : _filter.Text(paginationQuery.Search);
            SortDefinition<User> sort = null;
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

        public async Task<User> CreateUserAsync(User user)
        {
            await CreateAsync(user);
            return user;
        }

        public async Task DeleteUserAsync(string id)
        {
            await DeleteAsync(id);
            return;
        }
    }
}
