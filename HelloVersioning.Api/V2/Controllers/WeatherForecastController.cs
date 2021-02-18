using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace HelloVersioning.Api.V2.Controllers
{
    [ApiController]
    [FeatureGate("EnableVersionTwo")]
    [Route("v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IFeatureManager _featureManager;

        public WeatherForecastController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet]
        public IEnumerable<V2.Models.WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new V2.Models.WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                Temperature = rng.Next(-20, 55),
                TemperatureUnit = "C",
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
