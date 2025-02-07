using TravelRecommendationsAPI.Models;

public class UserFeedback
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int DestinationId { get; set; }

    // This references the FeedbackTypes table
    public int FeedbackId { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // The navigation property
    public virtual FeedbackType FeedbackType { get; set; }
}
