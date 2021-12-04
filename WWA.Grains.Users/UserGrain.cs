using AutoMapper;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;
using WWA.Grains.Constants;
using WWA.Grains.Users.Entities;

namespace WWA.Grains.Users
{
    public interface IUserGrain : IGrainWithStringKey
    {
        Task<UserState> GetUserAsync();
        Task<UserState> ReplaceUserAsync(UserState userState);
        Task DeleteUserAsync();
    }

    public class UserGrain : Grain<UserState>, IUserGrain
    {
        private readonly IPersistentState<UserState> _user;
        private readonly ILogger<IUserGrain> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserGrain(
            [PersistentState(MongoCollections.UserState, MongoStorageProviders.GrainState)]
            IPersistentState<UserState> user,
            ILogger<UserGrain> logger,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _user = user;
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public override async Task OnActivateAsync()
        {
            var primaryKey = this.GetPrimaryKeyString();
            if (_user.State.Id == null)
            {
                User user = await _userRepository.GetAsync(primaryKey);
                _user.State = _mapper.Map<UserState>(user);
            }
            _user.State.DateActive = DateTime.UtcNow;
            await _user.WriteStateAsync();
            await base.OnActivateAsync();
            return;
        }

        public Task<UserState> GetUserAsync()
        {
            if (State == null)
            {
                throw new Exception($"Unable to load User with id: {this.GetPrimaryKeyString()}");
            }
            return Task.FromResult(_user.State);
        }

        public Task<UserState> ReplaceUserAsync(UserState userState)
        {
            _user.State.DateModified = DateTime.UtcNow;
            _user.State.DisplayName = userState.DisplayName;
            _user.State.Email = userState.Email;

            _user.WriteStateAsync();

            return Task.FromResult(_user.State);
        }

        public Task DeleteUserAsync()
        {
            _user.ClearStateAsync();
            this.DeactivateOnIdle();
            return Task.CompletedTask;
        }
    }
}
