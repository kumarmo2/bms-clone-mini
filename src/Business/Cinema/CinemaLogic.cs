using BMS.DataAccess.Cinema;
using BMS.Models.Cinema;
using cm = BMS.Models.Cinema;
using cdto = BMS.Dtos.Cinema;

namespace BMS.Business.Cinema;

public class CinemaLogic : ICinemaLogic
{
    private readonly IAuditoriumRepository _auditoriumRepository;
    private readonly ICinemaRepository _cinemaRepository;

    public CinemaLogic(IAuditoriumRepository auditoriumRepository, ICinemaRepository cinemaRepository)
    {
        _auditoriumRepository = auditoriumRepository;
        _cinemaRepository = cinemaRepository;
    }

    public async Task<Auditorium> GetAuditorium(int id)
    {
        // TODO: add checks 
        return await _auditoriumRepository.Get(id);
    }

    public async Task<cm.Cinema> GetCinemaForAudi(int audiId)
    {
        return await _auditoriumRepository.GetCinemaForAudi(audiId);
    }

    public async Task<IEnumerable<cdto.AudiCinema>> GetCinemasForAudis(IEnumerable<int> audiIds)
    {
        return await _cinemaRepository.GetAudiCinemas(audiIds);
    }
}

