using Microsoft.AspNetCore.Mvc;
using Products.Interface;
using Products.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await productService.GetAll();
            return new OkObjectResult(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await productService.Get(id);

            if (item == null)
            {
                return NotFound(id);
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = await productService.GetAll();

            if (products.Any(o => o.Id == product.Id))
            {
                return BadRequest($"Product with ID {product.Id} already exists!");
            }

            var item = await productService.Create(product);
            return CreatedAtAction("Create", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await productService.Get(id) == null)
            {
                return NotFound(id);
            }

            var item = await productService.Update(id, product);
            return new OkObjectResult(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var products = await productService.GetAll();
            if (!products.Any(o => o.Id == id))
                return NotFound(id);

            var item = await productService.Delete(id);
            return new OkObjectResult(item);
        }
    }
}
