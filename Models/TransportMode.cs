using System.Text.Json.Serialization;

namespace TravelRecommendationsAPI.Models
{
    public class TransportMode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore] // Avoid cycle

        public virtual ICollection<User> Users { get; set; } = new List<User>();
        [JsonIgnore] // Avoid cycle

        public virtual ICollection<Destination> Destinations { get; set; } = new List<Destination>();
    }
}
