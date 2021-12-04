using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;
using WWA.Grains.Constants;
using WWA.Grains.Games.Entities;

namespace WWA.Grains.Games
{
    public interface IGameGrain : IGrainWithStringKey
    {
        Task<GameState> GetGameAsync();
        Task<GameState> UpdateGameAsync(JsonPatchDocument<GameState> gameStateOperations);
        Task DeleteGameAsync();
    }

    public class GameGrain : Grain<GameState>, IGameGrain
    {
        private readonly IPersistentState<GameState> _game;
        private readonly ILogger<IGameGrain> _logger;
        private readonly IMapper _mapper;
        private readonly IGameRepository _gameRepository;

        public GameGrain(
            [PersistentState(MongoCollections.GameState, MongoStorageProviders.GrainState)]
            IPersistentState<GameState> game,
            ILogger<GameGrain> logger,
            IMapper mapper,
            IGameRepository gameRepository)
        {
            _game = game;
            _logger = logger;
            _mapper = mapper;
            _gameRepository = gameRepository;
        }

        public override async Task OnActivateAsync()
        {
            var primaryKey = this.GetPrimaryKeyString();
            if (_game.State.Id == null)
            {
                Game game = await _gameRepository.GetGameAsync(primaryKey);
                _game.State = _mapper.Map<GameState>(game);
            }
            await _game.WriteStateAsync();
            await base.OnActivateAsync();
            return;
        }

        public Task<GameState> GetGameAsync()
        {
            if (State == null)
            {
                throw new Exception($"Unable to load Game with id: {this.GetPrimaryKeyString()}");
            }
            return Task.FromResult(_game.State);
        }

        public Task<GameState> UpdateGameAsync(JsonPatchDocument<GameState> gameStateOperations)
        {
            GameState game = _game.State;
            try
            {
                gameStateOperations.ApplyTo(game);
            }
            catch
            {
                throw;
            }

            _game.State = game;
            _game.State.DateModified = DateTime.UtcNow;
            _game.WriteStateAsync();

            return Task.FromResult(_game.State);
        }

        public Task DeleteGameAsync()
        {
            _game.ClearStateAsync();
            this.DeactivateOnIdle();
            return Task.CompletedTask;
        }
    }
}
