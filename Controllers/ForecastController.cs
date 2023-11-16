using System;
using Forecast.Entities;
using Forecast.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Forecast.Controllers
{
    [Route("Forecast")]
    public class ForecastController : Controller
    {
        private readonly IForecastRepository forecastRepository;

        public ForecastController(IForecastRepository forecastRepository)
        {
            this.forecastRepository = forecastRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string city)
        {
            return Ok(await forecastRepository.GetForecastsAsync(city));
        }

        [HttpPost("Create")]
        public IActionResult Post([FromBody] WeatherForecast forecast)
        {
            try
            {
                forecastRepository.SaveAsync(forecast);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
