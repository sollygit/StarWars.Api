using System.Text.Json.Serialization;

namespace Products.Model
{
    public class MovieDetails : Movie
    {
        [JsonPropertyName("rated")]
        public string Rated { get; set; }
        [JsonPropertyName("released")]
        public string Released { get; set; }
        [JsonPropertyName("runtime")]
        public string Runtime { get; set; }
        [JsonPropertyName("genre")]
        public string Genre { get; set; }
        [JsonPropertyName("director")]
        public string Director { get; set; }
        [JsonPropertyName("writer")]
        public string Writer { get; set; }
        [JsonPropertyName("language")]
        public string Language { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("awards")]
        public string Awards { get; set; }
        [JsonPropertyName("metascore")]
        public int Metascore { get; set; }
        [JsonPropertyName("rating")]
        public decimal Rating { get; set; }
        [JsonPropertyName("votes")]
        public string Votes { get; set; }
    }
}
