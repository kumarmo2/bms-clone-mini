namespace BMS.DataAccess.Cinema;

using BMS.Models.Cinema;
using CommonLibs.Database;
using Dapper;
using cm = BMS.Models.Cinema;

public class AuditoriumRepository : IAuditoriumRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AuditoriumRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Auditorium> Get(int id)
    {
        var query = "select * from cinema.auditoriums where id = @id limit 1";

        using (var conn = _connectionFactory.GetDbConnection())
        {
            return await conn.QueryFirstOrDefaultAsync<Auditorium>(query, new { id = id });
        }
    }

    public async Task<cm.Cinema> GetCinemaForAudi(int audiId)
    {
        var query = $@"select cm.* from cinema.cinemas cm inner join cinema.auditoriums au
                      on cm.id = au.cinemaid where au.id = @audiid limit 1";
        using (var conn = _connectionFactory.GetDbConnection())
        {
            return await conn.QueryFirstOrDefaultAsync<cm.Cinema>(query, new { audiid = audiId });
        }
    }
}

