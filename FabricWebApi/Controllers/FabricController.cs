using FabricWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FabricWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FabricController : ControllerBase
    {
        private readonly IFabricService _fabricService;

        public FabricController(IFabricService fabricService)
        {
            _fabricService = fabricService;
        }

        [HttpGet]
        public IActionResult ExecuteCommand()
        {
            var output = _fabricService.ExecuteCommand();
            return Ok(output);
        }
    }
}
