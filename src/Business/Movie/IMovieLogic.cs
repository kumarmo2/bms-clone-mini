using BMS.Dtos.Movie;
using CommonLibs.Utils;
using mm = BMS.Models.Movie;

namespace BMS.Business.Movie;


public interface IMovieLogic
{

    Task<OneOf<mm.Movie, string>> CreateMovie(CreateMovieRequest request);
    Task<mm.Movie> GetMovie(long id);
    Task<IEnumerable<mm.Movie>> GetMovies(IEnumerable<long> ids);
}

