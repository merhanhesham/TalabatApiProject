using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
    [Route("error/{Code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
       
        public ActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code));
        }
    }
}
