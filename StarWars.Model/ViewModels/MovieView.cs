using System.Collections.Generic;

namespace StarWars.Model.ViewModels
{
    public class MovieView
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
        public decimal Price { get; set; }
        public ICollection<MovieDetailsView> MovieDetails { get; set; } = new List<MovieDetailsView>();
    }
}
