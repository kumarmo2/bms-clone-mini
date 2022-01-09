using mm = BMS.Models.Movie;
namespace BMS.DataAccess.Movie;


public interface IMovieRepository
{
    Task Create(mm.Movie movie);
    Task<mm.Movie> GetByName(string name);
    Task<mm.Movie> GetById(long id);
    Task<IEnumerable<mm.Movie>> GetByIds(IEnumerable<long> ids);
}

