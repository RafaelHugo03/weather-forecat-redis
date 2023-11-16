using System;
using Forecast.Entities;

namespace Forecast.Repositories.Interfaces
{
    public interface IForecastRepository
    {
        Task SaveAsync(WeatherForecast forecast);
        Task<WeatherForecast> GetForecastsAsync(string city);
    }
}
