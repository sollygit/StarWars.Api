using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarWars.Api.Settings;
using StarWars.Common;
using StarWars.Interface;
using StarWars.Model;
using StarWars.Model.ViewModels;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StarWars.Api.Services
{
    public class WebJetService : IWebJetService
    {
        private readonly ILogger<WebJetService> logger;
        private readonly IMemoryCache cache;
        private readonly WebJetSettings settings;
        private readonly HttpClient httpClient;

        public WebJetService(
            ILogger<WebJetService> logger,
            IMemoryCache cache,
            WebJetSettings settings,
            HttpClient httpClient)
        {
            this.logger = logger;
            this.cache = cache;
            this.settings = settings;
            this.httpClient = httpClient;
        }

        public Task<MovieViewModel[]> GetAll(string provider)
        {
            // Cache results per provider
            return cache.GetOrCreateAsync(provider, async entry => {
                entry.SlidingExpiration = TimeSpan.FromMinutes(settings.Cache);
                return await GetAllAsync(provider);
            });
        }

        private async Task<MovieViewModel[]> GetAllAsync(string provider)
        {
            // Inject header token
            var uriBuilder = new UriBuilder($"{settings.BaseUrl}/{provider}/movies");
            var response = await httpClient.GetAsync(uriBuilder.Uri);
            var jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            var movies = ((JArray)jObject["Movies"]).Select(o => {
                var movie = JsonConvert.DeserializeObject<Movie>(o.ToString());
                return Mapper.Map<MovieViewModel>(movie);
            });

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"Something went wrong: {response.StatusCode}");
                throw new ServiceException(response.StatusCode, $"Get Movies failed for provider {provider}");
            }

            return movies.ToArray();
        }

        public async Task<MovieDetailsViewModel> Get(string provider, string id)
        {
            var uriBuilder = new UriBuilder($"{settings.BaseUrl}/{provider}/movie/{id}");
            var response = await httpClient.GetAsync(uriBuilder.Uri);
            var result = response.Content.ReadAsStringAsync().Result;
            var movie = JsonConvert.DeserializeObject<MovieDetails>(result);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"Something went wrong: {response.StatusCode}");
                throw new ServiceException(response.StatusCode, $"Get Movie failed for provider {provider} and id {id}");
            }

            return Mapper.Map<MovieDetailsViewModel>(movie);
        }
    }
}
