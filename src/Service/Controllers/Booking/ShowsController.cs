using BMS.Business.Booking;
using BMS.Dtos.Booking;
using Microsoft.AspNetCore.Mvc;

namespace BMS.Services.Controllers.Booking;


public class ShowsController : CommonController
{
    private readonly IShowLogic _showLogic;

    public ShowsController(IShowLogic showLogic)
    {
        _showLogic = showLogic;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShow(CreateShowRequest request)
    {
        if (request is null)
        {
            return BadRequest("request cannot be empty");
        }
        var result = await _showLogic.CreateShow(request);
        if (result is null || !result.Ok)
        {
            return StatusCode(500);
        }
        return Ok();
    }
}

