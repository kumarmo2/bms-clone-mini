using BMS.Business.Cinema;
using BMS.Business.Movie;
using BMS.DataAccess.Booking;
using BMS.Dtos.Booking;
using BMS.Models.Booking;
using BMS.Models.Cinema;
using mm = BMS.Models.Movie;
using cdto = BMS.Dtos.Cinema;
using CommonLibs.Utils;
using CommonLibs.Utils.Id;
using BMS.Business.Location;
using BMS.Models.Location;

namespace BMS.Business.Booking;

public class ShowLogic : IShowLogic
{
    private readonly ICinemaLogic _cinemaLogic;
    private readonly IIdFactory _idFactory;
    private readonly IMovieLogic _movieLogic;
    private readonly IShowRepository _showRepository;
    private readonly ILocationLogic _locationLogic;

    public ShowLogic(ICinemaLogic cinemaLogic, IIdFactory idFactory, IMovieLogic movieLogic,
            IShowRepository showRepository, ILocationLogic locationLogic)
    {
        _cinemaLogic = cinemaLogic;
        _idFactory = idFactory;
        _movieLogic = movieLogic;
        _showRepository = showRepository;
        _locationLogic = locationLogic;
    }

    private async Task<OneOf<(City, IEnumerable<MovieShowOverview>), string>> GetCityAndMovieOverviews(int cityId)
    {
        var now = DateTime.UtcNow;

        var cityTask = _locationLogic.GetCity(cityId);
        var showsTask = _showRepository.GetMoviesOverviewForCity(cityId, now);

        await Task.WhenAll(cityTask, showsTask);

        var city = cityTask.Result;

        if (city is null || city.Id < 0)
        {
            return new OneOf<(City, IEnumerable<MovieShowOverview>), string>($"No city found, cityId: {cityId}");
        }

        var shows = showsTask.Result;
        return new OneOf<(City, IEnumerable<MovieShowOverview>), string>((city, shows));
    }

    private async Task PopulateMovieNames(IEnumerable<MovieShowOverview> shows)
    {
        var movieIds = shows.Select(show => show.MovieId);
        var movies = await _movieLogic.GetMovies(movieIds);
        if (movies is null || !movies.Any())
        {
            return;
        }

        var moviesMap = movies.ToDictionary(movie => movie.Id);
        foreach (var show in shows)
        {
            var movieId = show.MovieId;
            moviesMap.TryGetValue(movieId, out var movie);
            if (movie is null)
            {
                continue;
            }
            show.MovieName = movie.Name;
        }

    }

    private async Task<CityShows> GetCityShows(int cityId, City city, IEnumerable<MovieShowOverview> shows)
    {
        var result = new CityShows
        {
            CityId = cityId,
            CityName = city.Name,
            Shows = shows
        };

        await PopulateMovieNames(shows);

        return result;
    }

    public async Task<OneOf<CityShows, string>> GetShowsForCity(int cityId)
    {
        if (cityId < 1)
        {
            throw new ArgumentException($"Invalid cityId: {cityId}");
        }

        var cityShowsTuple = await GetCityAndMovieOverviews(cityId);
        if (cityShowsTuple.Err is not null)
        {
            return new OneOf<CityShows, string>(cityShowsTuple.Err);
        }

        var (city, shows) = cityShowsTuple.Ok;

        if (shows == null || !shows.Any())
        {
            var cityShows = new CityShows
            {
                CityId = cityId,
                CityName = city.Name
            };
            return new OneOf<CityShows, string>(cityShows);
        }

        var result = await GetCityShows(cityId, city, shows);
        return new OneOf<CityShows, string>(result);
    }

    private async Task<OneOf<(IEnumerable<Show>, mm.Movie), string>> GetMovieAndShows(long movieId)
    {
        var showsTask = _showRepository.GetAvailableShows(movieId, DateTime.UtcNow);
        var movieTask = _movieLogic.GetMovie(movieId);

        await Task.WhenAll(showsTask, movieTask);
        var movie = movieTask.Result;

        if (movie is null || movie.Id != movieId)
        {
            return new OneOf<(IEnumerable<Show>, mm.Movie), string>("No Movie Found");
        }

        Console.WriteLine($"movie: {movie.Name}");

        var shows = showsTask.Result;
        return new OneOf<(IEnumerable<Show>, mm.Movie), string>((shows, movie));
    }

    private async Task<Dictionary<int, cdto.AudiCinema>> GetAudiIdToAudiCinemaMap(IEnumerable<int> audiIds)
    {
        var audiCinemas = await _cinemaLogic.GetCinemasForAudis(audiIds);

        if (audiCinemas is null || !audiCinemas.Any())
        {
            // Ideally this should never happen.
            throw new Exception("Internal Server Error. Please Try Again");
        }

        // cinemas could have duplicate cinemas as multiple audis can belong to
        // same cinema.

        var audiIdToCinemaMap = new Dictionary<int, cdto.AudiCinema>();

        foreach (var audiCinema in audiCinemas)
        {
            if (audiIdToCinemaMap.ContainsKey(audiCinema.AudiId))
            {
                continue;
            }
            Console.WriteLine($"setting audiid to ciname, audiId: {audiCinema.AudiId}, >>> {audiCinema.CinemaId}, name>>> {audiCinema.CinemaName}");
            audiIdToCinemaMap[audiCinema.AudiId] = audiCinema;
        }
        return audiIdToCinemaMap;
    }

    public async Task<OneOf<MovieShows, string>> GetMovieShows(long movieId)
    {
        if (movieId < 1)
        {
            throw new ArgumentException($"Invalid {movieId}");
        }

        var movieShowsTuple = await GetMovieAndShows(movieId);
        if (movieShowsTuple.Err is not null)
        {
            return new OneOf<MovieShows, string>(movieShowsTuple.Err);
        }
        var (shows, movie) = movieShowsTuple.Ok;

        if (shows is null || !shows.Any())
        {
            Console.WriteLine($"no shows found");
            return new OneOf<MovieShows, string>(new MovieShows { MovieId = movie.Id, MovieName = movie.Name });
        }

        var audiIds = shows.Select(show => show.AudiId);
        var audiIdToCinemaMap = await GetAudiIdToAudiCinemaMap(audiIds);

        var showOverviews = shows.Select(show =>
        {
            audiIdToCinemaMap.TryGetValue(show.AudiId, out var audiCinema);
            return new MovieShowOverview
            {
                MovieId = movieId,
                CinemaName = audiCinema?.CinemaName,
                CinemaId = audiCinema is not null ? audiCinema.CinemaId : 0,
                ShowId = show.Id,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
            };
        });

        var movieShows = new MovieShows
        {
            MovieId = movieId,
            MovieName = movie.Name,
            Shows = showOverviews
        };

        return new OneOf<MovieShows, string>(movieShows);
    }

    public async Task<OneOf<ShowInfo, string>> GetShowInfo(long showId)
    {
        // TODO: do validation in controller.
        var show = await _showRepository.GetShowById(showId);

        if (show is null || show.Id < 1)
        {
            return new OneOf<ShowInfo, string>("No show could be found");
        }

        var seatAvailabilitiesTask = _showRepository.GetShowSeatAvailablities(showId);
        var auditoriumTask = _cinemaLogic.GetAuditorium(show.AudiId);
        var movieTask = _movieLogic.GetMovie(show.MovieId);
        var cinemaTask = _cinemaLogic.GetCinemaForAudi(show.AudiId);

        await Task.WhenAll(seatAvailabilitiesTask, auditoriumTask, movieTask, cinemaTask);

        var seatAvailabilities = seatAvailabilitiesTask.Result;
        var audi = auditoriumTask.Result;

        if (seatAvailabilities is null || audi is null)
        {
            throw new Exception("Something went wrong. Please try again!");
        }

        var seatAvailablityMatrix = GenerateSeatAvailabilityMatrix(audi.Layout, seatAvailabilities, audi.NumOfRows, audi.NumOfCols);
        var cinema = cinemaTask.Result;
        var movie = movieTask.Result;

        var showInfo = new ShowInfo
        {
            AudiId = audi.Id,
            Id = show.Id,
            MovieId = show.MovieId,
            AudiName = audi.Name,
            CinemaName = cinema?.Name,
            MovieName = movie?.Name,
            SeatAvailablityLayout = seatAvailablityMatrix,
        };
        return new OneOf<ShowInfo, string>(showInfo);
    }

    public async Task<OneOf<bool, string>> BookMyShow(BookShowRequest request, long userId)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        // TODO: add validations like, num of seats booked cannot be more than 10.

        return await _showRepository.BookMyShow(request, userId);
        // get show seat availablities and check the requested 
    }

    private static string GetSeatAvaibilityKey(ShowSeatAvailablity seatAvailablity)
    {
        return $"{GetSeatAvaibilityKey(seatAvailablity.RowIndex, seatAvailablity.ColIndex)}";
    }

    private static string GetSeatAvaibilityKey(int rowIndex, int colIndex)
    {
        return $"row:{rowIndex}|col:{colIndex}";
    }

    private static List<List<int>> GenerateSeatAvailabilityMatrix(bool[,] layout,
            IEnumerable<ShowSeatAvailablity> seatAvailablities, int numOfRows, int numOfCols)
    {

        // -1 represents seat is booked.
        // 0 represents there is not seat there.
        // 1 represents seat is available.
        var matrix = new List<List<int>>();

        var seatAvailabilityMap = seatAvailablities.ToDictionary(sa => GetSeatAvaibilityKey(sa));

        for (var i = 0; i < numOfRows; i++)
        {
            var newRow = new List<int>();
            for (var j = 0; j < numOfCols; j++)
            {
                var hasSeat = layout[i, j];

                if (!hasSeat)
                {
                    newRow.Add(0);
                    continue;
                }
                var seatAvailibilityKey = GetSeatAvaibilityKey(i, j);
                if (!seatAvailabilityMap.ContainsKey(seatAvailibilityKey))
                {
                    // Ideally this case will never happen unless there is a data issue.
                    newRow.Add(0);
                    continue;
                }
                var seatAvailablity = seatAvailabilityMap[seatAvailibilityKey];
                if (seatAvailablity.IsBooked)
                {
                    newRow.Add(-1);
                    continue;
                }
                newRow.Add(1);
            }
            matrix.Add(newRow);
        }
        return matrix;
    }

    private async Task<OneOf<(mm.Movie, Auditorium), string>> GetMovieAndAudi(CreateShowRequest request)
    {

        var audiTask = _cinemaLogic.GetAuditorium(request.AudiId);
        var movieTask = _movieLogic.GetMovie(request.MovieId);

        await Task.WhenAll(audiTask, movieTask);

        var audi = audiTask.Result;
        var movie = movieTask.Result;

        if (audi is null || audi.Id < 1)
        {
            return new OneOf<(mm.Movie, Auditorium), string>("No audi found with the given audi id");
        }

        if (movie is null || movie.Id < 1)
        {
            return new OneOf<(mm.Movie, Auditorium), string>("No movie found");
        }

        if (request.StartTime < movie.ReleaseDate)
        {
            return new OneOf<(mm.Movie, Auditorium), string>("Cannot create show before the release date");
        }

        return new OneOf<(mm.Movie, Auditorium), string>((movie, audi));
    }

    public async Task<OneOf<Show, string>> CreateShow(CreateShowRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var movieAudiTuple = await GetMovieAndAudi(request);
        if (!string.IsNullOrWhiteSpace(movieAudiTuple.Err))
        {
            return new OneOf<Show, string>(movieAudiTuple.Err);
        }
        var (movie, audi) = movieAudiTuple.Ok;

        // If any show already exists colliding with the new show, send error.
        var collidingShow = await _showRepository.GetShowForAudiBetweenTime(audi.Id, request.StartTime, request.EndTime);
        if (collidingShow is not null && collidingShow.Id > 0)
        {
            return new OneOf<Show, string>($"Cannot create show as there is another scheduled between {collidingShow.StartTime} and {collidingShow.EndTime} in audi: {audi.Name} ");
        }

        var show = await CreateShow(request, audi);
        return new OneOf<Show, string>(show);
    }

    private async Task<Show> CreateShow(CreateShowRequest request, Auditorium audi)
    {
        var show = GetShow(request);

        await _showRepository.CreateShow(show);

        var seatAvailabilities = GenerateShowSeatAvailablities(show.Id, audi);
        try
        {
            await _showRepository.CreateShowSeatAvailabilities(seatAvailabilities);
        }
        catch
        {
            // TODO: delete the show if any error occurs while creating seatAvailabilities.
            return null;
        }
        return show;
    }

    private List<ShowSeatAvailablity> GenerateShowSeatAvailablities(long showId, Auditorium audi)
    {
        var rows = audi.NumOfRows;
        var cols = audi.NumOfCols;

        var seatAvailabilities = new List<ShowSeatAvailablity>();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                if (audi.Layout[i, j] == false)
                {
                    continue;
                }
                var seatAvailability = new ShowSeatAvailablity
                {
                    Id = _idFactory.Next(),
                    ShowId = showId,
                    ColIndex = j,
                    RowIndex = i,
                    IsBooked = false
                };
                seatAvailabilities.Add(seatAvailability);
            }
        }
        return seatAvailabilities;
    }

    private Show GetShow(CreateShowRequest request)
    {
        return new Show
        {
            Id = _idFactory.Next(),
            AudiId = request.AudiId,
            EndTime = request.EndTime,
            StartTime = request.StartTime,
            MovieId = request.MovieId,
        };
    }

}


