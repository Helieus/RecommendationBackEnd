namespace TravelRecommendationsAPI.Models
{
    public class UserFeedback
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DestinationId { get; set; }
        public int FeedbackId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
