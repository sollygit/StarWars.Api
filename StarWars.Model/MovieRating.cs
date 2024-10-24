using FluentValidation;
using System;

namespace StarWars.Model
{
    public class MovieRating : AuditableEntity
    {
        public Guid ID { get; set; }
        public string MovieID { get; set; }
        public decimal Price { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public int Metascore { get; set; }
        public decimal Rating { get; set; }
        public string Votes { get; set; }
    }

    public class MovieRatingValidator : AbstractValidator<MovieRating>
    {
        public MovieRatingValidator()
        {
            RuleFor(x => x.ID).NotEmpty().WithMessage("ID cannot be empty");
            RuleFor(x => x.MovieID).NotEmpty().WithMessage("MovieID cannot be empty");
        }
    }
}
