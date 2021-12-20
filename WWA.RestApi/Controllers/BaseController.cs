using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace WWA.RestApi.Controllers
{
    public abstract class BaseController : Controller
    {
        public string UserId => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        public string UserEmail => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
    }
}
