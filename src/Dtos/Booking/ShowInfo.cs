namespace BMS.Dtos.Booking;

public class ShowInfo
{
    public long Id { get; set; }
    public long MovieId { get; set; }
    public string MovieName { get; set; }
    public int AudiId { get; set; }
    public string AudiName { get; set; }
    public string CinemaName { get; set; }
    public List<List<int>> SeatAvailablityLayout { get; set; }
}


