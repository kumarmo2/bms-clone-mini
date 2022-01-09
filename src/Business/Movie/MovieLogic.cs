namespace BMS.Business.Movie;

using CommonLibs.Utils;
using mm = BMS.Models.Movie;
using BMS.DataAccess.Movie;
using CommonLibs.Utils.Id;

using BMS.Dtos.Movie;
using System.Collections.Generic;

public class MovieLogic : IMovieLogic
{
    private readonly IMovieRepository _movieRepository;
    private readonly IIdFactory _idFactory;
    public MovieLogic(IMovieRepository movieRepository, IIdFactory idFactory)
    {
        _movieRepository = movieRepository;
        _idFactory = idFactory;
    }

    public async Task<OneOf<mm.Movie, string>> CreateMovie(CreateMovieRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        var existingMovie = await _movieRepository.GetByName(request.Name);

        if (existingMovie is not null && existingMovie.Id > 0)
        {
            return new OneOf<mm.Movie, string>("Movie with same name already exists");
        }

        var movie = GetMovieFromCreateMovieRequest(request);
        await _movieRepository.Create(movie);

        return new OneOf<mm.Movie, string>(movie);
    }

    public async Task<mm.Movie> GetMovie(long id)
    {
        return await _movieRepository.GetById(id);
    }

    public async Task<IEnumerable<mm.Movie>> GetMovies(IEnumerable<long> ids)
    {
        if (ids == null)
        {
            throw new ArgumentNullException(nameof(ids));
        }
        return await _movieRepository.GetByIds(ids);
    }

    private mm.Movie GetMovieFromCreateMovieRequest(CreateMovieRequest request)
    {
        return new mm.Movie
        {
            Id = _idFactory.Next(),
            Name = request.Name,
            ReleaseDate = request.ReleaseDate,
            DirectorName = request.DirectorName
        };
    }

}

