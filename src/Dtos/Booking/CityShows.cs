namespace BMS.Dtos.Booking;

public class CityShows
{
    public int CityId { get; set; }
    public string CityName { get; set; }
    public IEnumerable<MovieShowOverview> Shows { get; set; }
}

