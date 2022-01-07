namespace BMS.DataAccess.Cinema;
using cm = BMS.Models.Cinema;
using cdto = BMS.Dtos.Cinema;

public interface ICinemaRepository
{
    Task<cm.Cinema> Get(int id);
    Task<IEnumerable<cdto.AudiCinema>> GetAudiCinemas(IEnumerable<int> audiIds);
}

