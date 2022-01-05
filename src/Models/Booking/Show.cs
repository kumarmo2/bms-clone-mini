namespace BMS.Models.Booking;

public class Show
{
    public long Id { get; set; }
    public long MovieId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int AudiId { get; set; }
}

