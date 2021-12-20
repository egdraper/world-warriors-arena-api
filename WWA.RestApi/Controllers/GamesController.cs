using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WWA.GrainInterfaces;
using WWA.GrainInterfaces.Models;
using WWA.RestApi.ViewModels.Games;

namespace WWA.RestApi.Controllers
{

    [Authorize]
    [Route("games")]
    [ApiController]
    [SwaggerTag]
    public class GamesController : BaseController
    {

        private readonly IClusterClient _clusterClient;
        private readonly ILogger<GamesController> _logger;
        private readonly IMapper _mapper;

        public GamesController(
            IClusterClient clusterClient,
            ILogger<GamesController> logger,
            IMapper mapper)
        {
            _clusterClient = clusterClient;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpHead]
        public async Task<ActionResult> QueryGames(
            [FromQuery] string name)
        {
            var gameService = _clusterClient.GetGrain<IGameService>(Guid.Empty);

            var xTotalCount = await gameService.QueryGamesAsync(UserId, name);

            HttpContext.Response.Headers.Add("X-Total-Count", xTotalCount.ToString());
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameSummaryViewModel>>> GetGames(
            [FromQuery] PaginationQueryModel paginationQuery)
        {
            var gameService = _clusterClient.GetGrain<IGameService>(Guid.Empty);
            PaginatedEntityModel<GameModel> games;
            try
            {
                games = await gameService.GetGamesAsync(UserId, paginationQuery);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            HttpContext.Response.Headers.Add("X-Total-Count", games.TotalCount.ToString());

            return Ok(_mapper.Map<IEnumerable<GameSummaryViewModel>>(games.Page));
        }

        [HttpGet("{id}", Name = "GetGame")]
        public async Task<ActionResult<GameReadViewModel>> GetGame(
            [FromRoute] string id)
        {
            var gameService = _clusterClient.GetGrain<IGameService>(Guid.Empty);
            var game = await gameService.GetGameAsync(UserId, id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GameReadViewModel>(game));
        }

        [HttpPost]
        public async Task<ActionResult<GameReadViewModel>> CreateGame(
            [FromBody] GameCreateViewModel gameCreateViewModel)
        {
            var gameService = _clusterClient.GetGrain<IGameService>(Guid.Empty);
            var gameModel = _mapper.Map<GameModel>(gameCreateViewModel);
            var createdGame = await gameService.CreateGameAsync(UserId, gameModel);

            return Created($"/games/{createdGame.Id}", _mapper.Map<GameReadViewModel>(createdGame));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GameReadViewModel>> UpdateGame(
            [FromRoute] string id,
            [FromBody] JsonPatchDocument<GameUpdateViewModel> gameUpdateOperations)
        {
            var gameService = _clusterClient.GetGrain<IGameService>(Guid.Empty);
            var gameApplyTo = _mapper.Map<GameUpdateViewModel>(await gameService.GetGameAsync(UserId, id));
            try
            {
                gameUpdateOperations.ApplyTo(gameApplyTo, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var gameToUpdate = _mapper.Map<GameUpdateModel>(gameApplyTo);
                var updatedGame = await gameService.UpdateGameAsync(UserId, id, gameToUpdate);
                return Ok(_mapper.Map<GameReadViewModel>(updatedGame));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGame(
            [FromRoute] string id)
        {
            var gameService = _clusterClient.GetGrain<IGameService>(Guid.Empty);
            await gameService.DeleteGameAsync(UserId, id);

            return NoContent();
        }
    }
}
