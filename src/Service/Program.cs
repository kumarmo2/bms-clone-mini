using CommonLibs.Utils.Id;
using CommonLibs.Database;
using BMS.DataAccess.Movie;
using BMS.Business.Movie;
using BMS.DataAccess.Cinema;
using BMS.Business.Cinema;
using BMS.DataAccess.Booking;
using BMS.Business.Booking;
using BMS.Business.User;
using BMS.DataAccess.User;
using BMS.Utils.User;
using BMS.Services.Filters;
using BMS.DataAccess.Location;
using BMS.Business.Location;

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
services.AddSingleton<ICinemaRepository, CinemaRepository>();
services.AddSingleton<IShowRepository, ShowRepository>();
services.AddSingleton<IShowLogic, ShowLogic>();
services.AddSingleton<IUserLogic, UserLogic>();
services.AddSingleton<IUserRepository, UserRepository>();
services.AddSingleton<IUserUtils, UserUtils>();
services.AddSingleton<AuthFilter>();
services.AddSingleton<ICityRepository, CityRepository>();
services.AddSingleton<ILocationLogic, LocationLogic>();
services.AddUserSecrets(config);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
