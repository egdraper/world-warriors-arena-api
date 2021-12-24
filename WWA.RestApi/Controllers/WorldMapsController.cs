using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WWA.GrainInterfaces;
using WWA.GrainInterfaces.Models;
using WWA.RestApi.ViewModels.Maps;

namespace WWA.RestApi.Controllers
{
    [Authorize]
    [Route("games/{gameId}/worldMaps")]
    [ApiController]
    [SwaggerTag]
    public class WorldMapsController : BaseController
    {

        private readonly IClusterClient _clusterClient;
        private readonly ILogger<WorldMapsController> _logger;
        private readonly IMapper _mapper;

        public WorldMapsController(
            IClusterClient clusterClient,
            ILogger<WorldMapsController> logger,
            IMapper mapper)
        {
            _clusterClient = clusterClient;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpHead]
        public async Task<ActionResult> QueryWorldMaps(
            [FromRoute] string gameId,
            [FromQuery] string name)
        {
            var worldMapService = _clusterClient.GetGrain<IWorldMapService>(gameId);

            var xTotalCount = await worldMapService.QueryMapsAsync(UserId, name);

            HttpContext.Response.Headers.Add("X-Total-Count", xTotalCount.ToString());
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorldMapSummaryViewModel>>> GetWorldMaps(
            [FromRoute] string gameId,
            [FromQuery] PaginationQueryModel paginationQuery)
        {
            var worldMapService = _clusterClient.GetGrain<IWorldMapService>(gameId);
            PaginatedEntityModel<WorldMapModel> worldMaps;
            try
            {
                worldMaps = await worldMapService.GetMapsAsync(UserId, paginationQuery);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            HttpContext.Response.Headers.Add("X-Total-Count", worldMaps.TotalCount.ToString());

            return Ok(_mapper.Map<IEnumerable<WorldMapSummaryViewModel>>(worldMaps.Page));
        }

        [HttpGet("{id}", Name = "GetWorldMap")]
        public async Task<ActionResult<WorldMapReadViewModel>> GetWorldMap(
            [FromRoute] string gameId,
            [FromRoute] string id)
        {
            var worldMapService = _clusterClient.GetGrain<IWorldMapService>(gameId);
            var worldMap = await worldMapService.GetMapAsync(UserId, id);

            if (worldMap == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WorldMapReadViewModel>(worldMap));
        }

        [HttpPost]
        public async Task<ActionResult<WorldMapReadViewModel>> CreateWorldMap(
            [FromRoute] string gameId,
            [FromBody] WorldMapCreateViewModel mapCreateViewModel)
        {
            var worldMapService = _clusterClient.GetGrain<IWorldMapService>(gameId);
            var worldMapModel = _mapper.Map<WorldMapModel>(mapCreateViewModel);
            var createdMap = await worldMapService.CreateMapAsync(UserId, worldMapModel);

            return Created($"/worldMaps/{createdMap.Id}", _mapper.Map<WorldMapReadViewModel>(createdMap));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<WorldMapReadViewModel>> UpdateWorldMap(
            [FromRoute] string gameId,
            [FromRoute] string id,
            [FromBody] JsonPatchDocument<WorldMapUpdateViewModel> worldMapUpdateOperations)
        {
            try
            {
                var worldMapService = _clusterClient.GetGrain<IWorldMapService>(gameId);
                var mapApplyTo = _mapper.Map<WorldMapUpdateViewModel>(await worldMapService.GetMapAsync(UserId, id));
                worldMapUpdateOperations.ApplyTo(mapApplyTo, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var mapToUpdate = _mapper.Map<WorldMapUpdateModel>(mapApplyTo);
                var updatedMap = await worldMapService.UpdateMapAsync(UserId, id, mapToUpdate);
                return Ok(_mapper.Map<WorldMapReadViewModel>(updatedMap));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorldMap(
            [FromRoute] string gameId,
            [FromRoute] string id)
        {
            var worldMapService = _clusterClient.GetGrain<IWorldMapService>(gameId);
            await worldMapService.DeleteMapAsync(UserId, id);

            return NoContent();
        }
    }
}
