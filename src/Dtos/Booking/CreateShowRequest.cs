namespace BMS.Dtos.Booking;

public class CreateShowRequest
{
    public long MovieId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int AudiId { get; set; }
}

