namespace BMS.Dtos.Booking;

public class MovieShows
{
    public long MovieId { get; set; }
    public string MovieName { get; set; }
    public IEnumerable<MovieShowOverview> Shows { get; set; }
}


