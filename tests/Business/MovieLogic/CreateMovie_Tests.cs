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
using BMS.Business.Movie;
using mm = BMS.Models.Movie;
using BMS.Tests;
using BMS.DataAccess.Movie;

public class CreateMovie_Tests
{
    [Theory, AutoDomainData]
    public async Task Null_Request_Throws_Exception(MovieLogic sut)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateMovie(null));
    }

    public async Task Movie_Name_Already_Exists_Returns_Error(Mock<IMovieRepository> movieRepositoryStub, mm.Movie existingMovie, MovieLogic sut)
    {
        // movieRepositoryStub.Setup(mr => mr.GetByName(It.IsAny<string>())).ReturnsAsync<mm.Movie>(existingMovie);
        // movieRepositoryStub.Setup(mr => mr.GetByName(It.IsAny<string>())).ReturnsAsync<mm.Movie>(existingMovie);


    }
}

