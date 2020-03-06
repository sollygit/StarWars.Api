using Products.Model;
using System.Threading.Tasks;

namespace Products.Interface
{
    public interface IMovieService
    {
        Task<Movie[]> GetAll(string provider);
        Task<MovieDetails> Get(string provider, string id);
    }
}
