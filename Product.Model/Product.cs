using FluentValidation;
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

    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product ID cannot be empty");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product Name cannot be empty")
                .MaximumLength(100).WithMessage("Product Name maximum length is 100");
            RuleForEach(x => x.ProductOptions)
                .SetValidator(new ProductOptionValidator());
        }
    }
}
