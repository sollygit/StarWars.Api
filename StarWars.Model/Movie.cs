using FluentValidation;
using System;
using System.Collections.Generic;

namespace StarWars.Model
{
    public class Movie : AuditableEntity
    {
        public Guid MovieID { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public ICollection<MovieDetails> MovieDetails { get; set; } = new List<MovieDetails>();
    }

    public class MovieValidator : AbstractValidator<Movie>
    {
        public MovieValidator()
        {
            RuleFor(x => x.MovieID)
                .NotEmpty().WithMessage("MovieID cannot be empty");
            RuleFor(x => x.ID)
                .NotEmpty().WithMessage("ID cannot be empty");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty")
                .MaximumLength(100).WithMessage("Title maximum length is 100");
            RuleForEach(x => x.MovieDetails)
                .SetValidator(new MovieDetailsValidator());
        }
    }
}
