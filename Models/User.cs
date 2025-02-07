namespace TravelRecommendationsAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string BudgetLevel { get; set; }
        public string TransportMode { get; set; }
        public string PreferredDestinationType { get; set; }
        public string PreferredActivities { get; set; }
        public string PreferredAccommodation { get; set; }
        public string CuisineImportance { get; set; }
        public string TourismStyle { get; set; }
        public string TripDuration { get; set; }
        public string TravelGroup { get; set; }
        public string SceneryVibe { get; set; }
    }
}
