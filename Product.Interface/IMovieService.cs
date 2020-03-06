using Products.Model.ViewModels;
using System.Threading.Tasks;

namespace Products.Interface
{
    public interface IMovieService
    {
        Task<MovieViewModel[]> GetAll(string provider);
        Task<MovieDetailsViewModel> Get(string provider, string id);
    }
}
