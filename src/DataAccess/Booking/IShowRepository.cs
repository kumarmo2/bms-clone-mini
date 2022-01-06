using BMS.Models.Booking;

namespace BMS.DataAccess.Booking;

public interface IShowRepository
{
    Task CreateShow(Show show);
    Task CreateShowSeatAvailabilities(IEnumerable<ShowSeatAvailablity> seatAvailablities);
    Task<Show> GetShowById(long id);
    Task<IEnumerable<ShowSeatAvailablity>> GetShowSeatAvailablities(long showId);
    Task<Show> GetShowForAudiBetweenTime(int audiId, DateTime startTime, DateTime endTime);
}

