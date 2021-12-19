using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WWA.GrainInterfaces;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Games.Entities;

namespace WWA.Grains.Games
{
    public class GameService : Grain<Game>, IGameService
    {
        private readonly IClusterClient _clusterClient;
        private readonly ILogger<IGameService> _logger;
        private readonly IMapper _mapper;
        private readonly IGameRepository _gameRepository;

        public GameService(
            IClusterClient clusterClient,
            ILogger<GameService> logger,
            IMapper mapper,
            IGameRepository gameRepository)
        {
            _clusterClient = clusterClient;
            _logger = logger;
            _mapper = mapper;
            _gameRepository = gameRepository;
        }

        public async Task<int> QueryGamesAsync(string ownedBy, string name)
        {
            int count = await _gameRepository.ExistsAsync(ownedBy, name);
            return count;
        }

        public async Task<PaginatedEntityModel<GameModel>> GetGamesAsync(string userId, PaginationQueryModel paginationQuery)
        {
            PaginatedEntityModel<GameModel> gameModels = new PaginatedEntityModel<GameModel>();
            try
            {
                var page = await _gameRepository.ListGamesPagedAsync(userId, paginationQuery);
                gameModels.TotalCount = page.TotalCount;
                gameModels.Page = _mapper.Map<IList<GameModel>>(page.Page);
            }
            catch
            {
                throw;
            }
            return gameModels;
        }

        public async Task<GameModel> GetGameAsync(string userId, string id)
        {
            var gameGrain = _clusterClient.GetGrain<IGameGrain>(id);
            var game = await gameGrain.GetGameAsync();

            return _mapper.Map<GameModel>(game);
        }

        public async Task<GameModel> CreateGameAsync(string userId, GameModel gameModel)
        {
            gameModel.CreatedBy = userId;
            gameModel.OwnedBy = userId;
            gameModel.Players = new List<string> { userId };

            int count = await _gameRepository.ExistsAsync(gameModel.OwnedBy, gameModel.Name);
            if (count > 0)
            {
                throw new Exception($"Game named {gameModel.Name} already exists");
            }
            var game = _mapper.Map<Game>(gameModel);
            await _gameRepository.CreateGameAsync(game);

            var gameGrain = _clusterClient.GetGrain<IGameGrain>(game.Id);
            var newGameModel = await gameGrain.GetGameAsync();

            return _mapper.Map<GameModel>(newGameModel);
        }

        public async Task<GameModel> UpdateGameAsync(string userId, string id, GameUpdateModel gameUpdateModel)
        {
            var gameGrain = _clusterClient.GetGrain<IGameGrain>(id);
            var newGameState = await gameGrain.UpdateGameAsync(userId, gameUpdateModel);

            return _mapper.Map<GameModel>(newGameState);
        }

        public async Task DeleteGameAsync(string userId, string id)
        {
            var gameGrain = _clusterClient.GetGrain<IGameGrain>(id);
            await gameGrain.DeleteGameAsync();
            await _gameRepository.DeleteGameAsync(id);
            return;
        }
    }
}
