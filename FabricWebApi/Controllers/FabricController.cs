using FabricWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FabricWebApi.Controllers;

[ApiController]
[Authorize]
[CheckBlockedUsers]
[Route("[controller]")]
public class FabricController : ControllerBase
{
    private readonly IFabricService _fabricService;

    public FabricController(IFabricService fabricService)
    {
        _fabricService = fabricService;
    }

    [HttpGet]
    public IActionResult AskFabric([FromBody] FabricRequest request)
    {
        var username = HttpContext.User.Identity.Name;
        var output = _fabricService.AskFabric(username, request.Request, request.Pattern, request.Session);
        return Ok(output);
    }

    [Authorize(Roles = Roles.Administrator)]
    [HttpGet("Sessions")]
    public IActionResult GetSessions()
    {
        return Ok(_fabricService.GetSessions());
    }

    [Authorize(Roles = Roles.Administrator)]
    [HttpPost("WipeSession/{session}")]
    public IActionResult WipeSession(string session)
    {
        _fabricService.WipeSession(session);

        return Ok();
    }
}
