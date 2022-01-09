using BMS.Models.Location;

namespace BMS.DataAccess.Location;

public interface ICityRepository
{
    Task<City> GetCity(int cityId);
}

