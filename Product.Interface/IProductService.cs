using Products.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> Get(Guid id);
        Task<Product> Create(Product product);
        Task<Product> Update(Guid id, Product product);
        Task<Product> Delete(Guid id);
    }
}
