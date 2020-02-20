using Products.Interface;
using Products.Model;
using Products.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.Api.Services
{
    public class ProductService : IProductService
    {
        readonly IProductsRepository productRepo;

        public ProductService(IProductsRepository productRepo)
        {
            this.productRepo = productRepo;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await productRepo.All();
        }

        public async Task<Product> Get(Guid id)
        {
            return await productRepo.GetById(id);
        }

        public async Task<Product> Create(Product product)
        {
            return await productRepo.Create(product);
        }

        public async Task<Product> Update(Guid id, Product product)
        {
            return await productRepo.Update(id, product);
        }

        public async Task<Product> Delete(Guid id)
        {
            return await productRepo.Delete(id);
        }
    }
}
