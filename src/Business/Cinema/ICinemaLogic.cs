using BMS.Models.Cinema;

namespace BMS.Business.Cinema;


public interface ICinemaLogic
{
    Task<Auditorium> GetAuditorium(int id);
}

