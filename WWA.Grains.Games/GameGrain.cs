using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Constants;
using WWA.Grains.Games.Entities;

namespace WWA.Grains.Games
{
    public interface IGameGrain : IGrainWithStringKey
    {
        Task<GameState> GetGameAsync();
        Task<GameState> UpdateGameAsync(string userId, GameUpdateModel gameUpdateModel);
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
                _mapper.Map(game, _game.State);
                _game.State.Id = primaryKey;
            }
            _game.State.DateActive = DateTime.UtcNow;
            await _game.WriteStateAsync();
            await base.OnActivateAsync();
            return;
        }

        public Task<GameState> GetGameAsync()
        {
            if (_game.State == null)
            {
                throw new Exception($"Unable to load Game with id: {this.GetPrimaryKeyString()}");
            }
            return Task.FromResult(_game.State);
        }

        public Task<GameState> UpdateGameAsync(string userId, GameUpdateModel gameUpdateModel)
        {
            if (userId != _game.State.OwnedBy)
            {
                throw new Exception($"User '{userId}' does not have access to modify this game");
            }
            _mapper.Map(gameUpdateModel, _game.State);
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
