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

    public async Task<OneOf<bool, string>> CreateShow(CreateShowRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        // TODO: do validations in controller.
        //
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

        // TODO: check if any other show exists in the given audi, colliding the new startTime and endTime. 
        // if yes, return error

        // TODO: Refactor.
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
        return new OneOf<bool, string>(true);
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


