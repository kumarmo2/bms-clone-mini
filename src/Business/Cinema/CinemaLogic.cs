using BMS.DataAccess.Cinema;
using BMS.Models.Cinema;
using cm = BMS.Models.Cinema;

namespace BMS.Business.Cinema;

public class CinemaLogic : ICinemaLogic
{
    private readonly IAuditoriumRepository _auditoriumRepository;

    public CinemaLogic(IAuditoriumRepository auditoriumRepository)
    {
        _auditoriumRepository = auditoriumRepository;
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
}

