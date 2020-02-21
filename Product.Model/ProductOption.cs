using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Products.Model
{
    public class ProductOption
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductOptionValidator : AbstractValidator<ProductOption>
    {
        public ProductOptionValidator()
        {
            RuleFor(register => register.Id).NotEmpty().WithMessage("ProductOption ID cannot be empty");
            RuleFor(register => register.ProductId).NotEmpty().WithMessage("Product ID cannot be empty");
        }
    }
}
