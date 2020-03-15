using System;
using System.Collections.Generic;
using MyApp.ServiceModel.Types;
using ServiceStack;

namespace MyApp.ServiceModel
{
    [Authenticate()]
    [RequiredRole("Admin")]
    [Route("/forecast")]
    public class GetWeatherForecast : IReturn<List<WeatherForecast>>
    {
        public DateTime StartDate { get; set; }
    }
}
