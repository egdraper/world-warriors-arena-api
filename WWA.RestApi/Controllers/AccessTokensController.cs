using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Orleans;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WWA.Configuration;
using WWA.GrainInterfaces;
using WWA.GrainInterfaces.Models;
using WWA.RestApi.ViewModels.AccessTokens;

namespace WWA.RestApi.Controllers
{
    [AllowAnonymous]
    [Route("accessTokens")]
    [ApiController]
    [SwaggerTag("API for generating tokens used to authenticate other operations.")]
    public class AccessTokensController : Controller
    {
        private IClusterClient _clusterClient;
        private IdentityConfiguration _identityConfiguration;

        public AccessTokensController(
            IClusterClient clusterClient,
            IConfiguration configuration)
        {
            _clusterClient = clusterClient;
            ApiConfig config = configuration.Get<ApiConfig>();
            _identityConfiguration = config.Identity;
        }

        [Authorize]
        [HttpHead]
        public ActionResult TestAccessToken()
        {
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateAccessToken(
            [FromBody] AccessTokenCreateViewModel accessTokenCreateViewModel)
        {
            var userService = _clusterClient.GetGrain<IUserService>(Guid.Empty);
            UserModel user = null;

            try
            {
                user = await userService.AuthenticateUserAsync(
                                accessTokenCreateViewModel.Email,
                                accessTokenCreateViewModel.Password);
            }
            catch (Exception ex)
            {
                throw;
            }

            if (user == null) { return Unauthorized(); }

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_identityConfiguration.Secret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, accessTokenCreateViewModel.Email),
                    new Claim("scope", "wwa_restapi")
                }),
                Issuer = _identityConfiguration.Issuer,
                IssuedAt = DateTime.UtcNow,
                Audience = _identityConfiguration.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_identityConfiguration.TokenExpiryInMinutes),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            var accessToken = new AccessTokenReadViewModel
            {
                AccessToken = token
            };
            return Ok(accessToken);
        }
    }
}
