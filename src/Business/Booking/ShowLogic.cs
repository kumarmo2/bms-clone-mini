using BMS.Business.Cinema;
using BMS.Business.Movie;
using BMS.DataAccess.Booking;
using BMS.Dtos.Booking;
using BMS.Models.Booking;
using BMS.Models.Cinema;
using CommonLibs.Utils;
using CommonLibs.Utils.Id;

namespace BMS.Business.Booking;

public class ShowLogic : IShowLogic
{
    private readonly ICinemaLogic _cinemaLogic;
    private readonly IIdFactory _idFactory;
    private readonly IMovieLogic _movieLogic;
    private readonly IShowRepository _showRepository;

    public ShowLogic(ICinemaLogic cinemaLogic, IIdFactory idFactory, IMovieLogic movieLogic,
            IShowRepository showRepository)
    {
        _cinemaLogic = cinemaLogic;
        _idFactory = idFactory;
        _movieLogic = movieLogic;
        _showRepository = showRepository;
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

        await Task.WhenAll(seatAvailabilitiesTask, auditoriumTask, movieTask);

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

    public async Task<OneOf<bool, string>> CreateShow(CreateShowRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        var audiTask = _cinemaLogic.GetAuditorium(request.AudiId);
        var movieTask = _movieLogic.GetMovie(request.MovieId);

        await Task.WhenAll(audiTask, movieTask);

        var audi = audiTask.Result;
        var movie = movieTask.Result;

        if (audi is null || audi.Id < 1)
        {
            return new OneOf<bool, string>("No audi found with the given audi id");
        }

        if (movie is null || movie.Id < 1)
        {
            return new OneOf<bool, string>("No movie found");
        }

        // If any show already exists colliding with the new show, send error.
        var collidingShow = await _showRepository.GetShowForAudiBetweenTime(audi.Id, request.StartTime, request.EndTime);
        if (collidingShow is not null && collidingShow.Id > 0)
        {
            return new OneOf<bool, string>($"Cannot create show as there is another scheduled between {collidingShow.StartTime} and {collidingShow.EndTime} in audi: {audi.Name} ");
        }

        await CreateShow(request, audi);
        return new OneOf<bool, string>(true);
    }

    private async Task CreateShow(CreateShowRequest request, Auditorium audi)
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
        }
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


