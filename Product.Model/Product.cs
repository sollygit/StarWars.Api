using System;
using System.Collections.Generic;

namespace Products.Model
{
    public class Product : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DeliveryPrice { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProductOption> ProductOptions { get; set; } = new List<ProductOption>();
    }
}
