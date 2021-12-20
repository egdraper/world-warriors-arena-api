using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WWA.GrainInterfaces;
using WWA.GrainInterfaces.Models;
using WWA.RestApi.ViewModels.Users;

namespace WWA.RestApi.Controllers
{
    [Authorize]
    [Route("users")]
    [ApiController]
    [SwaggerTag("Admin API for managing Users. Will be moved or hidden at a future date.")]
    public class UsersController : BaseController
    {
        private readonly IClusterClient _clusterClient;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;

        public UsersController(
            IClusterClient clusterClient,
            ILogger<UsersController> logger,
            IMapper mapper)
        {
            _clusterClient = clusterClient;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpHead]
        public async Task<ActionResult> QueryUsers(
            [FromQuery] string email)
        {
            var userService = _clusterClient.GetGrain<IUserService>(Guid.Empty);
            
            var xTotalCount = await userService.QueryUsersAsync(email);
            
            HttpContext.Response.Headers.Add("X-Total-Count", xTotalCount.ToString());
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSummaryViewModel>>> GetUsers(
            [FromQuery] PaginationQueryModel paginationQuery)
        {
            var userService = _clusterClient.GetGrain<IUserService>(Guid.Empty);
            PaginatedEntityModel<UserModel> users;
            try
            {
                users = await userService.GetUsersAsync(paginationQuery);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            HttpContext.Response.Headers.Add("X-Total-Count", users.TotalCount.ToString());

            return Ok(_mapper.Map<IEnumerable<UserSummaryViewModel>>(users.Page));
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<UserReadViewModel>> GetUser(
            [FromRoute] string id)
        {
            var userService = _clusterClient.GetGrain<IUserService>(Guid.Empty);
            var user = await userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserReadViewModel>(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserReadViewModel>> CreateUser(
            [FromBody] UserCreateViewModel userCreateViewModel)
        {
            var userService = _clusterClient.GetGrain<IUserService>(Guid.Empty);
            var userModel = _mapper.Map<UserModel>(userCreateViewModel);
            var createdUser = await userService.CreateUserAsync(userModel);

            return Created($"/users/{createdUser.Id}", _mapper.Map<UserReadViewModel>(createdUser));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserReadViewModel>> ReplaceUser(
            [FromRoute] string id,
            [FromBody] UserReplaceViewModel userReplaceViewModel)
        {
            var userService = _clusterClient.GetGrain<IUserService>(Guid.Empty);
            var userModel = _mapper.Map<UserModel>(userReplaceViewModel);
            userModel.Id = id;
            var replacedUser = await userService.ReplaceUserAsync(userModel);

            return Ok(_mapper.Map<UserReadViewModel>(replacedUser));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(
            [FromRoute] string id)
        {
            var userService = _clusterClient.GetGrain<IUserService>(Guid.Empty);
            await userService.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
