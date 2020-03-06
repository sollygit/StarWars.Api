using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Products.Api.Settings;
using Products.Interface;
using Products.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Products.Api.Services
{
    public class MovieService : IMovieService
    {
        private readonly ILogger<MovieService> logger;
        private readonly IMemoryCache cache;
        private readonly MovieSettings settings;

        public MovieService(ILogger<MovieService> logger, IMemoryCache cache, MovieSettings settings)
        {
            this.logger = logger;
            this.cache = cache;
            this.settings = settings;
        }

        public Task<IEnumerable<Movie>> GetAll(string provider)
        {
            // Cache results per provider
            return cache.GetOrCreateAsync(provider, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(settings.Cache);
                return await GetAllAsync(provider);
            });
        }

        private async Task<IEnumerable<Movie>> GetAllAsync(string provider)
        {
            using var client = new HttpClient();

            // Inject header token
            client.DefaultRequestHeaders.Add("x-access-token", settings.AccessToken);

            var uriBuilder = new UriBuilder($"{settings.BaseUrl}/{provider}/movies");
            var response = await client.GetAsync(uriBuilder.Uri);
            var jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            var movies = ((JArray)jObject["Movies"]).Select(o => {
                return new Movie
                {
                    ID = o["ID"].Value<string>(),
                    Title = o["Title"].Value<string>(),
                    Year = o["Year"].Value<string>(),
                    Type = o["Type"].Value<string>(),
                    Poster = o["Poster"].Value<string>(),
                    Price = GetRandomPrice() // Mockup a random price 
                };
            }).ToList();

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"Something went wrong: {response.StatusCode}");
                throw new ServiceException(response.StatusCode, $"Get Movies failed for provider {provider}");
            }

            return movies;
        }

        public async Task<MovieDetails> Get(string provider, string id)
        {
            using var client = new HttpClient();

            // Inject header token
            client.DefaultRequestHeaders.Add("x-access-token", settings.AccessToken);

            var uriBuilder = new UriBuilder($"{settings.BaseUrl}/{provider}/movie/{id}");
            var response = await client.GetAsync(uriBuilder.Uri);
            var result = response.Content.ReadAsStringAsync().Result;
            var movie = JsonConvert.DeserializeObject<MovieDetails>(result);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"Something went wrong: {response.StatusCode}");
                throw new ServiceException(response.StatusCode, $"Get Movie failed for provider {provider} and id {id}");
            }

            return movie;
        }

        private decimal GetRandomPrice()
        {
            return decimal.Parse(string.Format("{0:0.##}", new Random().NextDouble() * 1000));
        }
    }
}
