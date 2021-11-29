using AutoMapper;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WWA.GrainInterfaces;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Users.Entities;

namespace WWA.Grains.Users
{
    [StatelessWorker]
    public class UserService : Grain<User>, IUserService
    {
        private readonly IClusterClient _clusterClient;
        private readonly ILogger<IUserService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(
            IClusterClient clusterClient,
            ILogger<UserService> logger,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _clusterClient = clusterClient;
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserModel> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.AuthenticateUserAsync(email, password);
            return _mapper.Map<UserModel>(user);
        }

        public async Task<int> QueryUsersAsync(string email)
        {
            int count = await _userRepository.ExistsAsync(email);
            return count;
        }

        public async Task<PaginatedEntityModel<UserModel>> GetUsersAsync(PaginationQueryModel paginationQuery)
        {
            _logger.LogInformation($"\n Something happened");
            PaginatedEntityModel<UserModel> userModels = new PaginatedEntityModel<UserModel>();
            try
            {
                var page = await _userRepository.ListUsersPagedAsync(paginationQuery);
                userModels.TotalCount = page.TotalCount;
                userModels.Page = _mapper.Map<IList<UserModel>>(page.Page);
            }
            catch
            {
                throw;
            }
            return userModels;
        }

        public async Task<UserModel> GetUserAsync(string id)
        {
            var userGrain = _clusterClient.GetGrain<IUserGrain>(id);
            var user = await userGrain.GetUserAsync();

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> CreateUserAsync(UserModel userModel)
        {
            int count = await _userRepository.ExistsAsync(userModel.Email);
            if (count > 0)
            {
                throw new Exception("Email already registered to a user");
            }
            var user = _mapper.Map<User>(userModel);
            await _userRepository.CreateUserAsync(user);

            var userGrain = _clusterClient.GetGrain<IUserGrain>(user.Id);
            var newUserModel = await userGrain.GetUserAsync();

            return _mapper.Map<UserModel>(newUserModel);
        }

        public async Task<UserModel> ReplaceUserAsync(UserModel userModel)
        {
            var user = _mapper.Map<UserState>(userModel);

            var userGrain = _clusterClient.GetGrain<IUserGrain>(user.Id);
            var newUserModel = await userGrain.ReplaceUserAsync(user);

            return _mapper.Map<UserModel>(newUserModel);
        }

        public async Task DeleteUserAsync(string id)
        {
            var userGrain = _clusterClient.GetGrain<IUserGrain>(id);
            await userGrain.DeleteUserAsync();
            await _userRepository.DeleteUserAsync(id);
            return;
        }
    }
}
