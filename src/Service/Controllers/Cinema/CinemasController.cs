using System.Threading.Tasks;
using BMS.Business.Cinema;
using BMS.Dtos.Cinema;
using BMS.Models.Cinema;
using Microsoft.AspNetCore.Mvc;

namespace BMS.Services.Controllers.Cinema;

public class CinemasController : CommonController
{
    private readonly ICinemaLogic _cinemaLogic;
    public CinemasController(ICinemaLogic cinemaLogic)
    {
        _cinemaLogic = cinemaLogic;
    }

    [HttpGet("auditoriums/{id}")]
    public async Task<AuditoriumDto> GetAuditorium(int id)
    {
        var audi = await _cinemaLogic.GetAuditorium(id);
        if (audi is null)
        {
            return null;
        }
        return GetAuditoriumDto(audi);
    }

    private static AuditoriumDto GetAuditoriumDto(Auditorium audi)
    {
        return new AuditoriumDto
        {
            Id = audi.Id,
            Name = audi.Name,
            Layout = audi.Layout,
            CinemaId = audi.CinemaId,
        };
    }
}

