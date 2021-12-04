using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using WWA.GrainInterfaces;
using WWA.RestApi.ViewModels.CurrentContext;

namespace WWA.RestApi.Controllers
{
    [Authorize]
    [Route("currentContext")]
    [ApiController]
    [SwaggerTag]
    public class CurrentContextController : BaseController
    {
        private IClusterClient _clusterClient;
        private IMapper _mapper;

        public CurrentContextController(
            IClusterClient clusterClient,
            IMapper mapper)
        {
            _clusterClient = clusterClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CurrentContextReadViewModel>> GetCurrentContext()
        {
            var userService = _clusterClient.GetGrain<IUserService>(Guid.Empty);
            var user = await userService.GetUserAsync(UserId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CurrentContextReadViewModel>(user));
        }
    }
}
