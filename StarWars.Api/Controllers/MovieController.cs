using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StarWars.Interface;
using StarWars.Model;
using StarWars.Model.ViewModels;
using System.Threading.Tasks;

namespace StarWars.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService service;

        public MovieController(IMovieService service)
        {
            this.service = service;
        }

        [HttpGet()]
        public async Task<IActionResult> All()
        {
            var items = await service.All();
            return new OkObjectResult(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var item = await service.Get(id);
            if (item == null) return new NotFoundObjectResult(id);
            
            return new OkObjectResult(item);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] MovieView movieView)
        {
            var movie = Mapper.Map<Movie>(movieView);
            var item = await service.Create(movie);
            if (item == null) return new BadRequestObjectResult($"ID '{movieView.ID}' already exists in DB");
            
            return new OkObjectResult(item);
        }
    }
}