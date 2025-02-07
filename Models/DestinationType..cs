using System.Text.Json.Serialization;

namespace TravelRecommendationsAPI.Models
{
    public class DestinationType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]

        public virtual ICollection<User> Users { get; set; } = new List<User>();
        [JsonIgnore]

        public virtual ICollection<Destination> Destinations { get; set; } = new List<Destination>();
    }

}
