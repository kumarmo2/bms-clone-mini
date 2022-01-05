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
}

