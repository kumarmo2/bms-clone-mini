namespace BMS.Dtos.Booking;

public class BookShowRequest
{
    public long ShowId { get; set; }
    public List<Seat> Seats { get; set; }
}
