using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RateLimitedApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        [Route("status")]
        public async Task<string> GetStatus()
        {
            return await Task.FromResult("OK");
        }
    }
}
