using BMS.DataAccess.Location;
using BMS.Models.Location;

namespace BMS.Business.Location;

public class LocationLogic : ILocationLogic
{
    private readonly ICityRepository _cityRepository;

    public LocationLogic(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<City> GetCity(int cityId)
    {
        if (cityId < 1)
        {
            throw new ArgumentException("Invalid cityId");
        }
        return await _cityRepository.GetCity(cityId);
    }
}

