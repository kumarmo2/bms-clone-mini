using BMS.Models.Booking;

namespace BMS.DataAccess.Booking;

public interface IShowRepository
{
    Task CreateShow(Show show);
    Task CreateShowSeatAvailabilities(IEnumerable<ShowSeatAvailablity> seatAvailablities);
}

