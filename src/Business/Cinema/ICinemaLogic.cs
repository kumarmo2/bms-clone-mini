using BMS.Models.Cinema;
using cm = BMS.Models.Cinema;

namespace BMS.Business.Cinema;


public interface ICinemaLogic
{
    Task<Auditorium> GetAuditorium(int id);
    Task<cm.Cinema> GetCinemaForAudi(int audiId);
}

