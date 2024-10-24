﻿using Microsoft.EntityFrameworkCore;
using StarWars.Interface;
using StarWars.Model;

namespace StarWars.Repository
{
    public interface IMoviesRepository : IRepository<Movie>
    {
        Task<IEnumerable<Movie>> All();
        Task<Movie> Get(Guid movieID);
        Task<Movie> Create(Movie movie);
        Task<Movie> Update(string id, Movie movie);
        Task<Movie> Delete(string id);
    }

    public class MovieRepository : BaseRepository<Movie>, IMoviesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MovieRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Movie>> All()
        {
            return await _dbContext.Movies
                .Include(p => p.MovieRatings)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<Movie> Get(Guid movieID)
        {
            return await _dbContext.Movies
                .Include(p => p.MovieRatings)
                .SingleOrDefaultAsync(p => p.MovieID == movieID);
        }

        public async Task<Movie> Create(Movie movie)
        {
            var entry = _dbContext.Add(movie);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Movie> Update(string id, Movie entity)
        {
            var movie = _dbContext.Movies.Include(p => p.MovieRatings).Single(p => p.ID == id);
            var movieRatings = movie.MovieRatings;

            // Update the parent movie
            _dbContext.Entry(movie).CurrentValues.SetValues(entity);

            // Remove or update child collection items
            foreach (var rating in movieRatings)
            {
                var mvEntity = entity.MovieRatings.SingleOrDefault(o => o.ID == rating.ID);
                if (mvEntity != null)
                {
                    _dbContext.Entry(rating).CurrentValues.SetValues(mvEntity);
                }
                else
                {
                    _dbContext.Remove(rating);
                }
            }

            // Add new child collection items
            foreach (var md in entity.MovieRatings)
            {
                if (movieRatings.All(o => o.ID != md.ID))
                {
                    _dbContext.Add(md);
                }
            }

            await _dbContext.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie> Delete(string id)
        {
            var item = _dbContext.Movies.Single(o => o.ID == id);
            var entry = _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }
    }
}
