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
        private ApplicationDbContext _dbContext => (ApplicationDbContext)_context;

        public ProductsRepository(ILogger<ProductsRepository> _logger, ApplicationDbContext context) : base(context)
        {
            this._logger = _logger;
        }

        public async Task<IEnumerable<Product>> All()
        {
            return await _dbContext.Products
                .Include(p => p.ProductOptions)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _dbContext.Products
                .Include(p => p.ProductOptions)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> Create(Product product)
        {
            var entry = _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Product> Update(Guid id, Product entity)
        {
            var product = _dbContext.Products.Include(p => p.ProductOptions).Single(p => p.Id == id);
            var options = product.ProductOptions;

            // Update the parent product
            _dbContext.Entry(product).CurrentValues.SetValues(entity);

            // Remove or update child collection items
            foreach (var option in options)
            {
                var optionEntity = entity.ProductOptions.SingleOrDefault(o => o.Id == option.Id);
                if (optionEntity != null)
                {
                    _dbContext.Entry(option).CurrentValues.SetValues(optionEntity);
                }
                else
                {
                    _dbContext.Remove(option);
                }
            }

            // Add the new items
            foreach (var option in entity.ProductOptions)
            {
                if (options.All(o => o.Id != option.Id))
                {
                    options.Add(option);
                }
            }

            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product> Delete(Guid id)
        {
            var item = _dbContext.Products.Single(o => o.Id == id);
            var entry = _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }
    }
}
