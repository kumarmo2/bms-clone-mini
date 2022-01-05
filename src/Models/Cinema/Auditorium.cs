namespace BMS.Models.Cinema;

public class Auditorium
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CinemaId { get; set; }
    public bool[,] Layout { get; set; }
    public int NumOfRows { get; set; }
    public int NumOfCols { get; set; }
}


