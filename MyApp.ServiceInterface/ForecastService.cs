using System;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using MyApp.ServiceModel;
using MyApp.ServiceModel.Types;

namespace MyApp.ServiceInterface
{
    public class ForecastService : Service
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public object Any(GetWeatherForecast request)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = request.StartDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToList());
        }
    }
}
