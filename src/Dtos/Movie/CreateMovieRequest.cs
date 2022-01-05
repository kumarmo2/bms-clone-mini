namespace BMS.Dtos.Movie;

public class CreateMovieRequest
{
    public string Name { get; set; }
    public string DirectorName { get; set; }
    public DateTime ReleaseDate { get; set; }
}

