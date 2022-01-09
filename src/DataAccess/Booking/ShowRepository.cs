using BMS.Models.Booking;
using CommonLibs.Database;
using Dapper;
using CommonLibs.Utils;
using BMS.Dtos.Booking;

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

    public async Task<IEnumerable<MovieShowOverview>> GetMoviesOverviewForCity(int cityId, DateTime minEndTime)
    {
        var query = $@"select
                        sh.id as showid,
                        sh.movieid,
                        ci.id as cinemaid,
                        ci.name as cinemaname,
                        sh.starttime,
                        sh.endtime
                      from booking.shows sh
                      inner join
                        cinema.auditoriums au on au.id = sh.audiid
                      inner join
                        cinema.cinemas ci on au.cinemaid = ci.id
                      where
                        ci.cityid = @cityid and sh.endtime > @minendtime;
                      ";
        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            return await conn.QueryAsync<MovieShowOverview>(query, new { cityid = cityId, minendtime = minEndTime });
        }
    }

    public async Task<Show> GetShowForAudiBetweenTime(int audiId, DateTime startTime, DateTime endTime)
    {
        var query = $@"select * from booking.shows s where audiid = @audiid and
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
            return await conn.QueryFirstOrDefaultAsync<Show>(query, new { audiid = audiId, starttime = startTime, endtime = endTime });
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

    public async Task<IEnumerable<Show>> GetAvailableShows(long movieId, DateTime minEndTime)
    {
        var query = "select * from booking.shows where movieid = @movieid and endtime > @minendtime;";

        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            return await conn.QueryAsync<Show>(query, new { movieid = movieId, minendtime = minEndTime });
        }
    }

    private static string GenerateConditionForSelectingSeats(IEnumerable<Seat> seats)
    {
        var conditions = seats.Select(seat => $"(rowindex = {seat.RowIndex} and colindex = {seat.ColIndex})").ToList();
        var orJoinedConditions = string.Join(" or ", conditions);
        return orJoinedConditions;
    }

    public async Task<OneOf<bool, string>> BookMyShow(BookShowRequest request, long userId)
    {
        var showId = request.ShowId;
        var condtionsForSelectingSeats = GenerateConditionForSelectingSeats(request.Seats);
        var selectQuery = $@"select * from booking.showseatavailabilities
                            where
                                showid = @showid
                                and isbooked = false
                                and ({condtionsForSelectingSeats});";
        Console.WriteLine($"selectQuery: {selectQuery}");
        var updateQuery = $@"update booking.showseatavailabilities
                            set isbooked = true
                            where showid = @showid
                            and ({condtionsForSelectingSeats});";
        using (var conn = _dbConnectionFactory.GetDbConnection())
        {
            conn.Open();
            using (var transaction = conn.BeginTransaction())
            {
                var seatAvailibilities = await conn.QueryAsync<ShowSeatAvailablity>(selectQuery, new { showid = request.ShowId }, transaction: transaction);
                if (seatAvailibilities is null || seatAvailibilities.Count() < request.Seats.Count)
                {
                    transaction.Commit();
                    return new OneOf<bool, string>("Some of the seats are already booked. Try again!");
                }
                await conn.ExecuteAsync(updateQuery, new { showid = request.ShowId }, transaction);
                transaction.Commit();
                return new OneOf<bool, string>(true);
            }
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

