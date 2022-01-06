using BMS.Models.Cinema;
using cm = BMS.Models.Cinema;

namespace BMS.DataAccess.Cinema;


public interface IAuditoriumRepository
{
    Task<Auditorium> Get(int id);
    Task<cm.Cinema> GetCinemaForAudi(int audiId);
}

