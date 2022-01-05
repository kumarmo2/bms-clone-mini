namespace BMS.Dtos.Cinema;

public class AuditoriumDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CinemaId { get; set; }
    public bool[,] Layout { get; set; }
}



