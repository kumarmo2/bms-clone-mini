using movieModel = BMS.Models.Movie;
using Dapper;
using CommonLibs.Database;



namespace BMS.DataAccess.Movie;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public MovieRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Create(movieModel.Movie movie)
    {
        var query = $@"insert into movie.movies(id, name, directorname, releasedate) values (@id, @name, @directorname, @releasedate)";

        using (var conn = _connectionFactory.GetDbConnection())
        {
            await conn.ExecuteAsync(query, new
            {
                id = movie.Id,
                name = movie.Name,
                directorname = movie.DirectorName,
                releasedate = movie.ReleaseDate,
            });
        }
    }

    public async Task<movieModel.Movie> GetById(long id)
    {
        var query = $"select * from movie.movies where id = @id limit 1";
        using (var conn = _connectionFactory.GetDbConnection())
        {
            return await conn.QueryFirstOrDefaultAsync<movieModel.Movie>(query, new { id = id });
        }
    }

    public async Task<movieModel.Movie> GetByName(string name)
    {
        var query = $"select * from movie.movies where name = @name limit 1";
        using (var conn = _connectionFactory.GetDbConnection())
        {
            return await conn.QueryFirstOrDefaultAsync<movieModel.Movie>(query, new { name = name });
        }
    }
}

