using System;
using System.Text.Json;
using Forecast.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Forecast.Repositories.Interfaces
{
    public class ForecastRepository : IForecastRepository
    {
        private readonly IDistributedCache cache;
        private readonly string aggregateIndex = "forecast:";

        public ForecastRepository(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<WeatherForecast> GetForecastsAsync(string city)
        {
            var prefix = String.Concat(aggregateIndex, Convert.ToString(city.ToLower()));

            var entity = await cache.GetStringAsync(prefix);

            if(string.IsNullOrEmpty(entity)) return new();

            var forecasts = JsonSerializer.Deserialize<WeatherForecast>(entity);

            return forecasts;
        }


        public async Task SaveAsync(WeatherForecast forecast)
        {
            var prefix = String.Concat(aggregateIndex, Convert.ToString(forecast.City.ToLower()));

            var cacheConfig = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration =  forecast.Date.AddTicks(new TimeOnly(23, 59, 59).Ticks)
            };

            var entity = JsonSerializer.Serialize(forecast);

            await cache.SetStringAsync(prefix, entity, cacheConfig);
        }

    }
}
