using System.Text.Json.Serialization;
using TravelRecommendationsAPI.Models;

public class Destination
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    [JsonIgnore] 

    public int? BudgetLevelId { get; set; }
    public int? TransportModeId { get; set; }
    public int? DestinationTypeId { get; set; }
    public int? ActivitiesId { get; set; }
    public int? AccommodationTypeId { get; set; }
    public int? CuisineImportanceId { get; set; }
    public int? TourismStyleId { get; set; }
    public int? TripDurationId { get; set; }
    public int? TravelGroupId { get; set; }
    public int? SceneryVibeId { get; set; }

    // Navigation (virtual for lazy loading)
    public virtual BudgetLevel? BudgetLevel { get; set; }
    public virtual TransportMode? TransportMode { get; set; }
    public virtual DestinationType? DestinationType { get; set; }
    public virtual Activity? Activities { get; set; }
    public virtual AccommodationType? AccommodationType { get; set; }
    public virtual CuisineImportance? CuisineImportance { get; set; }
    public virtual TourismStyle? TourismStyle { get; set; }
    public virtual TripDuration? TripDuration { get; set; }
    public virtual TravelGroup? TravelGroup { get; set; }
    public virtual SceneryVibe? SceneryVibe { get; set; }
}