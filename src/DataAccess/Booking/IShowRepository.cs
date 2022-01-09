using BMS.Dtos.Booking;
using BMS.Models.Booking;
using CommonLibs.Utils;

namespace BMS.DataAccess.Booking;

public interface IShowRepository
{
    Task CreateShow(Show show);
    Task CreateShowSeatAvailabilities(IEnumerable<ShowSeatAvailablity> seatAvailablities);
    Task<Show> GetShowById(long id);
    Task<IEnumerable<ShowSeatAvailablity>> GetShowSeatAvailablities(long showId);
    Task<Show> GetShowForAudiBetweenTime(int audiId, DateTime startTime, DateTime endTime);
    Task<OneOf<bool, string>> BookMyShow(BookShowRequest request, long userId);
    Task<IEnumerable<Show>> GetAvailableShows(long movieId, DateTime minEndTime);
    Task<IEnumerable<MovieShowOverview>> GetMoviesOverviewForCity(int cityId, DateTime minEndTime);
}

