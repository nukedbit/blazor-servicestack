using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using MyApp.ServiceInterface;
using MyApp.ServiceModel;
using System;
using System.Collections.Generic;
using MyApp.ServiceModel.Types;

namespace MyApp.Tests
{
    public class UnitTest
    {
        private readonly ServiceStackHost appHost;

        public UnitTest()
        {
            appHost = new BasicAppHost().Init();
            appHost.Container.AddTransient<ForecastService>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        [Test]
        public void Can_call_MyServices()
        {
            var service = appHost.Container.Resolve<ForecastService>();

            var response = (List<WeatherForecast>)service.Any(new GetWeatherForecast { StartDate = DateTime.Now });

            Assert.That(response.Count, Is.EqualTo(5));
        }
    }
}
