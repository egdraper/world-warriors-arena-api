﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Constants;
using WWA.Grains.Games.Entities;
using WWA.Grains.Mongo;

namespace WWA.Grains.Games
{
    public interface IGameRepository
    {
        Task<int> ExistsAsync(string id = null, string playerId = null, string ownedBy = null, string name = null);
        Task<Game> GetGameAsync(string id);
        Task<PaginatedEntityModel<Game>> ListGamesPagedAsync(string userId, PaginationQueryModel paginationQuery);
        Task<Game> CreateGameAsync(Game game);
        Task DeleteGameAsync(string id);
    }

    public class GameRepository : Repository<Game>, IGameRepository
    {
        private ILogger<IGameRepository> _logger;
        private IMapper _mapper;
        public GameRepository(
            ILogger<GameRepository> logger,
            IMapper mapper,
            IMongoContext mongoContext) : base(mongoContext, MongoCollections.Games)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public Task<int> ExistsAsync(string id, string playerId, string ownedBy, string name)
        {
            List<FilterDefinition<Game>> filters = new();
            if (!string.IsNullOrWhiteSpace(id)) { filters.Add(_filter.Eq("_id", ObjectId.Parse(id))); }
            if (!string.IsNullOrWhiteSpace(playerId)) { filters.Add(_filter.AnyEq("Players", playerId)); }
            if (!string.IsNullOrWhiteSpace(ownedBy)) { filters.Add(_filter.Eq("OwnedBy", ownedBy)); }
            if (!string.IsNullOrWhiteSpace(name)) { filters.Add(_filter.Eq("Name", name)); }
            return QueryAsync(_filter.And(filters));
        }

        public async Task<Game> GetGameAsync(string id)
        {
            return await GetAsync(_filter.Eq("_id", ObjectId.Parse(id)));
        }

        public Task<PaginatedEntityModel<Game>> ListGamesPagedAsync(string userId, PaginationQueryModel paginationQuery)
        {
            var filter = string.IsNullOrEmpty(paginationQuery.Search)
                ? _filter.AnyEq("Players", userId)
                : _filter.And(_filter.In("Players", userId), _filter.Text(paginationQuery.Search));
            SortDefinition<Game> sort = null;
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

        public async Task<Game> CreateGameAsync(Game game)
        {
            await CreateAsync(game);
            return game;
        }

        public async Task DeleteGameAsync(string id)
        {
            await DeleteAsync(id);
            return;
        }
    }
}
