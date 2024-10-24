using Microsoft.EntityFrameworkCore;
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
                .Include(p => p.MovieDetails)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<Movie> Get(Guid movieID)
        {
            return await _dbContext.Movies
                .Include(p => p.MovieDetails)
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
            var movie = _dbContext.Movies.Include(p => p.MovieDetails).Single(p => p.ID == id);
            var movieDetails = movie.MovieDetails;

            // Update the parent movie
            _dbContext.Entry(movie).CurrentValues.SetValues(entity);

            // Remove or update child collection items
            foreach (var md in movieDetails)
            {
                var mvEntity = entity.MovieDetails.SingleOrDefault(o => o.ID == md.ID);
                if (mvEntity != null)
                {
                    _dbContext.Entry(md).CurrentValues.SetValues(mvEntity);
                }
                else
                {
                    _dbContext.Remove(md);
                }
            }

            // Add new child collection items
            foreach (var md in entity.MovieDetails)
            {
                if (movieDetails.All(o => o.ID != md.ID))
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
