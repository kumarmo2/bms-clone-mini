using BMS.Models.Location;
using CommonLibs.Database;
using Dapper;

namespace BMS.DataAccess.Location;

public class CityRepository : ICityRepository
{
    private IDbConnectionFactory _dbConnectionFactory;

    public CityRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<City> GetCity(int cityId)
    {
        var query = "select * from location.cities where id = @id";

        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            return await conn.QueryFirstOrDefaultAsync<City>(query, new { id = cityId });
        }
    }
}

