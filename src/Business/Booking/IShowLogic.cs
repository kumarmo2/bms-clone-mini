using BMS.Dtos.Booking;
using CommonLibs.Utils;

namespace BMS.Business.Booking;


public interface IShowLogic
{
    Task<OneOf<bool, string>> CreateShow(CreateShowRequest request);
    Task<OneOf<ShowInfo, string>> GetShowInfo(long showId);
    Task<OneOf<bool, string>> BookMyShow(BookShowRequest request, long userId);
}
