namespace BMS.DataAccess.Cinema;

using CommonLibs.Database;
using cm = BMS.Models.Cinema;
using Dapper;


public class CinemaRepository : ICinemaRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CinemaRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
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

