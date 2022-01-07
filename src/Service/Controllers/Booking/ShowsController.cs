using BMS.Business.Booking;
using BMS.Dtos.Booking;
using BMS.Services.Filters;
using BMS.Utils;
using BMS.Utils.User;
using CommonLibs.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BMS.Services.Controllers.Booking;


public class ShowsController : CommonController
{
    private readonly IShowLogic _showLogic;

    public ShowsController(IShowLogic showLogic)
    {
        _showLogic = showLogic;
    }

    [HttpGet("{movieId}")]
    public async Task<ActionResult<OneOf<MovieShows, string>>> GetShows(long movieId)
    {
        if (movieId < 0)
        {
            return BadRequest(new OneOf<MovieShows, string>("Invalid Movied"));
        }
        return await _showLogic.GetMovieShows(movieId);
    }

    [HttpPost]
    public async Task<ActionResult<OneOf<bool, string>>> CreateShow(CreateShowRequest request)
    {
        if (request is null)
        {
            return BadRequest("request cannot be empty");
        }
        var validationMessage = ValidateCreateShowRequest(request);
        if (validationMessage is not null)
        {
            return new OneOf<bool, string>(validationMessage);
        }
        var result = await _showLogic.CreateShow(request);
        if (result is null)
        {
            return StatusCode(500);
        }
        return result;
    }

    private static string ValidateCreateShowRequest(CreateShowRequest request)
    {
        if (request.AudiId < 1)
        {
            return $"Invalid {nameof(request.AudiId)}";
        }
        if (request.StartTime >= request.EndTime)
        {
            return "startTime cannot be greater than startTime";
        }
        return null;
    }

    [HttpGet("{showId}")]
    public async Task<ActionResult<OneOf<ShowInfo, string>>> GetShowInfo(long showId)
    {
        if (showId < 1)
        {
            return BadRequest(new OneOf<ShowInfo, string>("Invalid showId"));
        }
        return await _showLogic.GetShowInfo(showId);
    }

    [HttpPost("booking")]
    [ServiceFilter(typeof(AuthFilter))]
    public async Task<ActionResult<OneOf<bool, string>>> BookShow(BookShowRequest request)
    {
        var authDto = (UserAuthDto)Request.HttpContext.Items[Constants.AuthDtoKey];
        return await _showLogic.BookMyShow(request, authDto.UserId);
    }

}

