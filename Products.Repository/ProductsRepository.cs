using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Products.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Repository
{
    public interface IProductsRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> All();
        Task<Product> GetById(Guid id);
        Task<Product> Create(Product product);
        Task<Product> Update(Guid id, Product product);
        Task<Product> Delete(Guid id);
    }

    public class ProductsRepository : Repository<Product>, IProductsRepository
    {
        private readonly ILogger<ProductsRepository> _logger;
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;

        public ProductsRepository(ILogger<ProductsRepository> _logger, ApplicationDbContext context) : base(context)
        {
            this._logger = _logger;
        }

        public async Task<IEnumerable<Product>> All()
        {
            return await _appContext.Products
                .Include(p => p.ProductOptions)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _appContext.Products
                .Include(p => p.ProductOptions)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> Create(Product product)
        {
            var entry = _appContext.Add(product);
            await _appContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Product> Update(Guid id, Product item)
        {
            var product = await _appContext.Products.SingleAsync(p => p.Id == item.Id);
            var options = await _appContext.ProductOptions.Where(p => p.ProductId == item.Id).ToListAsync();

            // Update Product
            _appContext.Entry(product).CurrentValues.SetValues(item);

            // Update ProductOptions 
            foreach (var option in options)
            {
                var entry = item.ProductOptions.Single(o => o.Id == option.Id);
                _appContext.Entry(option).CurrentValues.SetValues(entry);
            }

            if (await _appContext.SaveChangesAsync() == 0)
            {
                _logger.LogError($"Update product {id} failed");
            }

            return product;
        }

        public async Task<Product> Delete(Guid id)
        {
            var item = _appContext.Products.Single(o => o.Id == id);
            var entry = _appContext.Remove(item);
            await _appContext.SaveChangesAsync();
            return entry.Entity;
        }
    }
}
