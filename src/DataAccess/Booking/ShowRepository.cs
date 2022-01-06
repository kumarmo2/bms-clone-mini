using BMS.Models.Booking;
using CommonLibs.Database;
using Dapper;

namespace BMS.DataAccess.Booking;

public class ShowRepository : IShowRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public ShowRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task CreateShow(Show show)
    {
        var query = @"insert into booking.shows (id, movieid, starttime, endtime, audiid)
                      values
                      (@id, @movieid, @starttime, @endtime, @audiid);";
        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            await conn.ExecuteAsync(query, new
            {
                id = show.Id,
                movieid = show.MovieId,
                starttime = show.StartTime,
                endtime = show.EndTime,
                audiid = show.AudiId
            });
        }
    }

    public async Task<Show> GetShowForAudiBetweenTime(int audiId, DateTime startTime, DateTime endTime)
    {
        var query = $@"select * from booking.shows s where audiid = 1 and
                        (
                          -- when the new interval lies completely inside the existing interval
                          (starttime <= @starttime and  endtime >= @endtime)
                          or
                          -- when the new interval completely engulfs the existing interval
                          (starttime >= @starttime and endtime <= @endtime)
                          or
                          -- when new endtime is between existing starttime and endtime
                          (endtime >= @endtime and @endtime >= starttime)
                          or
                          -- when new startitime is between existing starttime and endtime
                          (starttime < @starttime and @starttime < endtime)
                        );
                    ";
        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            return await conn.QueryFirstOrDefaultAsync<Show>(query, new { starttime = startTime, endtime = endTime });
        }
    }

    public async Task CreateShowSeatAvailabilities(IEnumerable<ShowSeatAvailablity> seatAvailablities)
    {
        var query = $@"insert into booking.showseatavailabilities(id, showid, rowindex, colindex, isbooked)
                      values
                      (@Id, @ShowId, @RowIndex, @ColIndex, @IsBooked)";
        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            await conn.ExecuteAsync(query, seatAvailablities);
        }
    }

    public async Task<Show> GetShowById(long id)
    {
        var query = "select * from booking.shows where id = @id limit 1";
        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            return await conn.QueryFirstOrDefaultAsync<Show>(query, new { id = id });
        }
    }

    public async Task<IEnumerable<ShowSeatAvailablity>> GetShowSeatAvailablities(long showId)
    {
        var query = "select * from booking.showseatavailabilities where showid = @showid";

        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            return await conn.QueryAsync<ShowSeatAvailablity>(query, new { showid = showId });
        }

    }
}

