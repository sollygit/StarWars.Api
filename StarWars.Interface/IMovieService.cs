using StarWars.Model.ViewModels;
using System;
using System.Threading.Tasks;

namespace StarWars.Interface
{
    public interface IMovieService
    {
        Task<MovieView[]> All();
        Task<MovieView> Get(Guid movieID);
    }
}
