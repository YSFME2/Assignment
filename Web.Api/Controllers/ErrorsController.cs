using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Route("/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetError()
        {
            var ex = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Console.WriteLine(ex.Error.Message);
            return Problem(ex.Error.Message);
        }
    }
}
