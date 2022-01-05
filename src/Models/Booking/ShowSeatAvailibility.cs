namespace BMS.Models.Booking;

public class ShowSeatAvailablity
{
    public long Id { get; set; }
    public long ShowId { get; set; }
    public int RowIndex { get; set; }
    public int ColIndex { get; set; }
    public bool IsBooked { get; set; }
}

