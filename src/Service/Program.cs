using CommonLibs.Utils.Id;
using CommonLibs.Database;
using BMS.DataAccess.Movie;
using BMS.Business.Movie;
using BMS.DataAccess.Cinema;
using BMS.Business.Cinema;
using BMS.DataAccess.Booking;
using BMS.Business.Booking;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;



var services = builder.Services;

services.AddIdFactory();
services.AddDatabase(config);
services.AddSingleton<IMovieLogic, MovieLogic>();
services.AddSingleton<IMovieRepository, MovieRepository>();
services.AddSingleton<IAuditoriumRepository, AuditoriumRepository>();
services.AddSingleton<ICinemaLogic, CinemaLogic>();
services.AddSingleton<IShowRepository, ShowRepository>();
services.AddSingleton<IShowLogic, ShowLogic>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
