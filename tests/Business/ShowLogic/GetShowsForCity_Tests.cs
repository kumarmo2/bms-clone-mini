using System.Threading.Tasks;
using Xunit;
using Moq;
using BMS.Business.Booking;
using System;
using AutoFixture.Xunit2;
using BMS.Business.Location;
using BMS.DataAccess.Booking;
using BMS.Models.Location;
using System.Collections.Generic;
using BMS.Dtos.Booking;

namespace BMS.Tests.Business.ShowLogic;

public class GetShowsForCity_Tests
{
    [Theory]
    [AutoDomainInlineDataAttribute(-1)]
    [AutoDomainInlineDataAttribute(0)]
    public async Task InvalidCityId_Throws_Exception(int cityId, BMS.Business.Booking.ShowLogic sut)
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => await sut.GetShowsForCity(cityId));
    }

    [Theory, AutoDomainData]
    public async Task CityRepository_Returns_No_City_Returns_Error([Frozen] Mock<ILocationLogic> locationLogicStub,
            [Frozen] Mock<IShowRepository> showRepositoryStub, BMS.Business.Booking.ShowLogic sut)
    {
        City city = null;
        locationLogicStub.Setup(ll => ll.GetCity(It.IsAny<int>())).ReturnsAsync(city);
        IEnumerable<MovieShowOverview> shows = null;
        showRepositoryStub.Setup(sr => sr.GetMoviesOverviewForCity(It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(shows);
        var cityId = 100;
        var result = await sut.GetShowsForCity(cityId);

        Assert.NotNull(result);
        Assert.NotNull(result.Err);
        Assert.Equal($"No city found, cityId: {cityId}", result.Err);
    }
}

