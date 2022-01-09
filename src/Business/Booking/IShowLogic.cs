using BMS.Dtos.Booking;
using BMS.Models.Booking;
using CommonLibs.Utils;

namespace BMS.Business.Booking;


public interface IShowLogic
{
    Task<OneOf<Show, string>> CreateShow(CreateShowRequest request);
    Task<OneOf<ShowInfo, string>> GetShowInfo(long showId);
    Task<OneOf<bool, string>> BookMyShow(BookShowRequest request, long userId);
    Task<OneOf<MovieShows, string>> GetMovieShows(long movieId);
    Task<OneOf<CityShows, string>> GetShowsForCity(int cityId);
}
