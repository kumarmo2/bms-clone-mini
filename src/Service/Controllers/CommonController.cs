using Microsoft.AspNetCore.Mvc;


namespace BMS.Services.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CommonController : ControllerBase
{
    protected ActionResult InternalServerError()
    {
        return StatusCode(500);
    }
}


