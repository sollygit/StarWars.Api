using Products.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.Interface
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAll(string provider);
        Task<MovieDetails> Get(string provider, string id);
    }
}
