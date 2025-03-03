using FabricWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FabricWebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class FabricController : ControllerBase
    {
        private readonly IFabricService _fabricService;

        public FabricController(IFabricService fabricService)
        {
            _fabricService = fabricService;
        }

        [HttpGet]
        public IActionResult CallFabric([FromBody] FabricRequest request)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var output = _fabricService.CallFabric(username, request.Request, request.Pattern);
            return Ok(output);
        }
    }
}
