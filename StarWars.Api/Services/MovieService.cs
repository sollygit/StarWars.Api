using AutoMapper;
using Microsoft.Extensions.Logging;
using StarWars.Interface;
using StarWars.Model.ViewModels;
using StarWars.Repository;
using System;
using System.Threading.Tasks;

namespace StarWars.Api.Services
{
    public class MovieService : IMovieService
    {
        private readonly ILogger<MovieService> logger;
        private readonly IMoviesRepository repo;

        public MovieService(
            ILogger<MovieService> logger,
            IMoviesRepository repo)
        {
            this.logger = logger;
            this.repo = repo;
        }

        public async Task<MovieViewModel[]> All()
        {
            var items = await repo.All();
            return Mapper.Map<MovieViewModel[]>(items);
        }

        public async Task<MovieViewModel> Get(Guid movieID)
        {
            var movie = await repo.Get(movieID);
            return Mapper.Map<MovieViewModel>(movie);
        }
    }
}
