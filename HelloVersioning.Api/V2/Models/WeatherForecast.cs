using System;

namespace HelloVersioning.Api.V2.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int Temperature { get; set; }

        public string Summary { get; set; }

        public string TemperatureUnit { get; set; }
    }
}
