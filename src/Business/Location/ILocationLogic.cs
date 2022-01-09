using BMS.Models.Location;

namespace BMS.Business.Location;

public interface ILocationLogic
{
    Task<City> GetCity(int cityId);
}
