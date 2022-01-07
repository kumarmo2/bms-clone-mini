namespace BMS.Dtos.Booking;


public class MovieShowOverview
{
    public long MovieId { get; set; }
    public long CinemaId { get; set; }
    public string CinemaName { get; set; }
    public long ShowId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

