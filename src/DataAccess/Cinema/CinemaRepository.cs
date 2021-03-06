namespace BMS.DataAccess.Cinema;

using CommonLibs.Database;
using cm = BMS.Models.Cinema;
using cdto = BMS.Dtos.Cinema;
using Dapper;


public class CinemaRepository : ICinemaRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CinemaRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<cdto.AudiCinema>> GetAudiCinemas(IEnumerable<int> audiIds)
    {
        var commaSeparatedAudiIds = string.Join(", ", audiIds.ToList());
        var query = $@"select c.id as cinemaid, c.name as cinemaname, a.id as audiid from cinema.cinemas c
                    inner join cinema.auditoriums a
                    on c.id = a.cinemaid and a.id in ({commaSeparatedAudiIds})";

        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            return await conn.QueryAsync<cdto.AudiCinema>(query);
        }
    }

    public async Task<cm.Cinema> Get(int id)
    {
        var query = "select * from cinema.cinemas where id = @id limit 1";

        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            return await conn.QueryFirstOrDefaultAsync<cm.Cinema>(query, new { id = id });
        }

    }
}

