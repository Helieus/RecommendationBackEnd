using TravelRecommendationsAPI.Models;

public class User
{
    public int Id { get; set; }
    public Guid SessionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? BudgetLevelId { get; set; }
    public int? TransportModeId { get; set; }
    public int? PreferredDestinationTypeId { get; set; }
    public int? PreferredActivitiesId { get; set; }
    public int? PreferredAccommodationId { get; set; }
    public int? CuisineImportanceId { get; set; }
    public int? TourismStyleId { get; set; }
    public int? TripDurationId { get; set; }
    public int? TravelGroupId { get; set; }
    public int? SceneryVibeId { get; set; }

    // Navigation (virtual for lazy loading)
    public virtual BudgetLevel? BudgetLevel { get; set; }
    public virtual TransportMode? TransportMode { get; set; }
    public virtual DestinationType? PreferredDestinationType { get; set; }
    public virtual Activity? PreferredActivities { get; set; }
    public virtual AccommodationType? PreferredAccommodation { get; set; }
    public virtual CuisineImportance? CuisineImportance { get; set; }
    public virtual TourismStyle? TourismStyle { get; set; }
    public virtual TripDuration? TripDuration { get; set; }
    public virtual TravelGroup? TravelGroup { get; set; }
    public virtual SceneryVibe? SceneryVibe { get; set; }
}