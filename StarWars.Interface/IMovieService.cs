using StarWars.Model.ViewModels;
using System;
using System.Threading.Tasks;

namespace StarWars.Interface
{
    public interface IMovieService
    {
        Task<MovieViewModel[]> All();
        Task<MovieViewModel> Get(Guid movieID);
    }
}
