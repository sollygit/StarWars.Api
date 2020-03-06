using System.Text.Json.Serialization;

namespace Products.Model
{
    public class Movie
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("year")]
        public string Year { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("poster")]
        public string Poster { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
