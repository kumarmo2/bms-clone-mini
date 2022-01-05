namespace BMS.DataAccess.Cinema;

using BMS.Models.Cinema;
using CommonLibs.Database;
using Dapper;

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
}

