using StarWars.Model.ViewModels;
using System.Threading.Tasks;

namespace StarWars.Interface
{
    public interface IWebJetService
    {
        Task<MovieView[]> GetAll(string provider);
        Task<MovieDetailsView> Get(string provider, string id);
    }
}
