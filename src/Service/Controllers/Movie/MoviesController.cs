using Microsoft.AspNetCore.Mvc;
using BMS.Dtos.Movie;
using BMS.Business.Movie;
using CommonLibs.Utils;
using mm = BMS.Models.Movie;
using System.Threading.Tasks;
using System;

namespace BMS.Services.Controllers.Movie;

public class MoviesController : CommonController
{
    private readonly IMovieLogic _movieLogic;
    public MoviesController(IMovieLogic movieLogic)
    {
        _movieLogic = movieLogic;
    }

    [HttpPost]
    public async Task<ActionResult<OneOf<MovieDto, string>>> CreateMovie(CreateMovieRequest request)
    {
        if (request is null)
        {
            return BadRequest("Body cannot be empty");
        }
        var validationMessage = ValidateCreateMovieRequest(request);

        if (validationMessage is not null)
        {
            return BadRequest(new OneOf<MovieDto, string>(validationMessage));
        }
        var result = await _movieLogic.CreateMovie(request);
        if (request is null)
        {
            return StatusCode(500);
        }
        if (!string.IsNullOrWhiteSpace(result.Err))
        {
            return new OneOf<MovieDto, string>(result.Err);
        }
        var movie = result.Ok;
        return new OneOf<MovieDto, string>(GetMovieDto(movie));
    }

    private static MovieDto GetMovieDto(mm.Movie movie)
    {
        return new MovieDto
        {
            Name = movie.Name,
            ReleaseDate = movie.ReleaseDate,
            Id = movie.Id,
            DirectorName = movie.DirectorName
        };
    }


    private static string ValidateCreateMovieRequest(CreateMovieRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return "Name cannot be empty";
        }
        if (request.ReleaseDate == default(DateTime))
        {
            return $"{nameof(request.ReleaseDate)} cannot be empty";
        }
        if (string.IsNullOrWhiteSpace(request.DirectorName))
        {
            return $"{nameof(request.DirectorName)} cannot be empty";
        }
        return null;
    }

    [HttpGet("{id}")]
    public ActionResult<string> GetMovie(long id)
    {
        // var result = Ok(new { id = id });
        // return Task.FromResult(Ok(new { x = " sfsf" }));
        return BadRequest("dfsf");
        // return Ok("dsf");
    }

}

