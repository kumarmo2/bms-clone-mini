namespace BMS.DataAccess.Cinema;
using cm = BMS.Models.Cinema;

public interface ICinemaRepository
{
    Task<cm.Cinema> Get(int id);
}

