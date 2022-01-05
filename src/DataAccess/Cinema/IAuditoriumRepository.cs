using BMS.Models.Cinema;

namespace BMS.DataAccess.Cinema;


public interface IAuditoriumRepository
{
    Task<Auditorium> Get(int id);
}

