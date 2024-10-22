using StarWars.Model.ViewModels;
using System.Threading.Tasks;

namespace StarWars.Interface
{
    public interface IMovieService
    {
        Task<MovieViewModel[]> GetAll(string provider);
        Task<MovieDetailsViewModel> Get(string provider, string id);
    }
}
