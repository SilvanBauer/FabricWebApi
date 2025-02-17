using FabricWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FabricWebApi.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IFabricService _fabricService;
        private readonly FabricWebApiDbContext _dbContext;

        public AdminController(IFabricService fabricService, FabricWebApiDbContext dbContext)
        {
            _fabricService = fabricService;
            _dbContext = dbContext;
        }

        [HttpGet("RecentRequests")]
        public IActionResult GetRecentRequests()
        {
            var recentRequests = _fabricService.GetRecentRequests();
            return Ok(recentRequests);
        }

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var users = _dbContext.ApplicationUsers.Select(au => $"{au.Id} - {au.Username}");
            return Ok(string.Join('\n', users));
        }
    }
}
