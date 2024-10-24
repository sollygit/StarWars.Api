using Microsoft.AspNetCore.Mvc;
using StarWars.Interface;
using System;
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

        [HttpGet("{movieID}")]
        public async Task<IActionResult> Get([FromRoute] Guid movieID)
        {
            var item = await service.Get(movieID);
            if (item == null) return new NotFoundObjectResult(movieID);
            
            return new OkObjectResult(item);
        }
    }
}