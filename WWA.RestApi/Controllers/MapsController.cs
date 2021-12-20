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
    [Route("maps")]
    [ApiController]
    [SwaggerTag]
    public class MapsController : BaseController
    {

        private readonly IClusterClient _clusterClient;
        private readonly ILogger<MapsController> _logger;
        private readonly IMapper _mapper;

        public MapsController(
            IClusterClient clusterClient,
            ILogger<MapsController> logger,
            IMapper mapper)
        {
            _clusterClient = clusterClient;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpHead]
        public async Task<ActionResult> QueryMaps(
            [FromQuery] string name)
        {
            var mapService = _clusterClient.GetGrain<IMapService>(Guid.Empty);

            var xTotalCount = await mapService.QueryMapsAsync(UserId, name);

            HttpContext.Response.Headers.Add("X-Total-Count", xTotalCount.ToString());
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MapSummaryViewModel>>> GetMaps(
            [FromQuery] PaginationQueryModel paginationQuery)
        {
            var mapService = _clusterClient.GetGrain<IMapService>(Guid.Empty);
            PaginatedEntityModel<MapModel> maps;
            try
            {
                maps = await mapService.GetMapsAsync(UserId, paginationQuery);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            HttpContext.Response.Headers.Add("X-Total-Count", maps.TotalCount.ToString());

            return Ok(_mapper.Map<IEnumerable<MapSummaryViewModel>>(maps.Page));
        }

        [HttpGet("{id}", Name = "GetMap")]
        public async Task<ActionResult<MapReadViewModel>> GetMap(
            [FromRoute] string id)
        {
            var mapService = _clusterClient.GetGrain<IMapService>(Guid.Empty);
            var map = await mapService.GetMapAsync(UserId, id);

            if (map == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MapReadViewModel>(map));
        }

        [HttpPost]
        public async Task<ActionResult<MapReadViewModel>> CreateMap(
            [FromBody] MapCreateViewModel mapCreateViewModel)
        {
            var mapService = _clusterClient.GetGrain<IMapService>(Guid.Empty);
            var mapModel = _mapper.Map<MapModel>(mapCreateViewModel);
            var createdMap = await mapService.CreateMapAsync(UserId, mapModel);

            return Created($"/maps/{createdMap.Id}", _mapper.Map<MapReadViewModel>(createdMap));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<MapReadViewModel>> UpdateMap(
            [FromRoute] string id,
            [FromBody] JsonPatchDocument<MapUpdateViewModel> mapUpdateOperations)
        {
            var mapService = _clusterClient.GetGrain<IMapService>(Guid.Empty);
            var mapApplyTo = _mapper.Map<MapUpdateViewModel>(await mapService.GetMapAsync(UserId, id));
            try
            {
                mapUpdateOperations.ApplyTo(mapApplyTo, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var mapToUpdate = _mapper.Map<MapUpdateModel>(mapApplyTo);
                var updatedMap = await mapService.UpdateMapAsync(UserId, id, mapToUpdate);
                return Ok(_mapper.Map<MapReadViewModel>(updatedMap));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMap(
            [FromRoute] string id)
        {
            var mapService = _clusterClient.GetGrain<IMapService>(Guid.Empty);
            await mapService.DeleteMapAsync(UserId, id);

            return NoContent();
        }
    }
}
