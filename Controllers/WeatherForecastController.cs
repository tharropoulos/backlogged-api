using Microsoft.AspNetCore.Mvc;
using backlogged_api.Data;
using backlogged_api.Models;
using Microsoft.EntityFrameworkCore;

namespace backlogged_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly BackloggedDBContext _backloggedDBContext;
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, BackloggedDBContext backloggedDBContext)
        {
            _logger = logger;
            _backloggedDBContext = backloggedDBContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<Game> Get()
        {

            return _backloggedDBContext.Games.Include(i => i.Franchise).Include(i => i.Publisher).Select(s => new Game
            {
                Title = s.Title,
                Id = s.Id,
                Franchise = s.Franchise,
                Publisher = s.Publisher,

            }).ToList();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
        }
    }
}