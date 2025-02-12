using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelRecommendationsAPI.Data;
using TravelRecommendationsAPI.DTOs;
using TravelRecommendationsAPI.Models;

namespace TravelRecommendationsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly TravelDbContext _context;

        public RecommendationController(TravelDbContext context)
        {
            _context = context;
        }

        // Combined endpoint: inserts the answers into a new record.
        [HttpPost("submit-answers")]
        public async Task<IActionResult> SubmitAnswers([FromBody] UserAnswersDto userAnswers)
        {
            // Create a new user record using the submitted answers.
            // The SessionId here is a dummy GUID since we’re not using sessions.
            var user = new User
            {
                SessionId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                BudgetLevelId = userAnswers.BudgetLevelId,
                TransportModeId = userAnswers.TransportModeId,
                PreferredDestinationTypeId = userAnswers.PreferredDestinationTypeId,
                PreferredActivitiesId = userAnswers.PreferredActivitiesId,
                PreferredAccommodationId = userAnswers.PreferredAccommodationId,
                CuisineImportanceId = userAnswers.CuisineImportanceId,
                TourismStyleId = userAnswers.TourismStyleId,
                TripDurationId = userAnswers.TripDurationId,
                TravelGroupId = userAnswers.TravelGroupId,
                SceneryVibeId = userAnswers.SceneryVibeId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Answers submitted successfully.");
        }

        // This GET endpoint picks the last inserted record and uses it for recommendations.
        [HttpGet("recommend")]
        public async Task<IActionResult> RecommendDestination()
        {
            // Get the last inserted user record.
            var user = await _context.Users.OrderByDescending(u => u.Id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("No user answers found.");
            }

            // Get all destinations and calculate scores.
            var allDestinations = await _context.Destinations.ToListAsync();
            var scoredDests = new List<(Destination Destination, int ContentScore, int CollaborativeScore)>();

            foreach (var dest in allDestinations)
            {
                int contentScore = CalculateContentScore(user, dest);
                int collaborativeScore = await CalculateCollaborativeScoreAsync(user, dest);
                scoredDests.Add((dest, contentScore, collaborativeScore));
            }

            var best = scoredDests
                .Select(s => new { Destination = s.Destination, TotalScore = s.ContentScore + s.CollaborativeScore })
                .OrderByDescending(x => x.TotalScore)
                .FirstOrDefault();

            if (best == null)
            {
                return Ok("No destinations found or scored.");
            }

            return Ok(best.Destination);
        }

        // Helper method to calculate content score.
        private int CalculateContentScore(User user, Destination dest)
        {
            int score = 0;
            if (user.BudgetLevelId == dest.BudgetLevelId) score += 10;
            if (user.TransportModeId == dest.TransportModeId) score += 10;
            if (user.PreferredDestinationTypeId == dest.DestinationTypeId) score += 10;
            if (user.PreferredActivitiesId == dest.ActivitiesId) score += 10;
            if (user.PreferredAccommodationId == dest.AccommodationTypeId) score += 10;
            if (user.CuisineImportanceId == dest.CuisineImportanceId) score += 10;
            if (user.TourismStyleId == dest.TourismStyleId) score += 10;
            if (user.TripDurationId == dest.TripDurationId) score += 10;
            if (user.TravelGroupId == dest.TravelGroupId) score += 10;
            if (user.SceneryVibeId == dest.SceneryVibeId) score += 10;
            return score;
        }

        // Helper method to calculate collaborative score.
        private async Task<int> CalculateCollaborativeScoreAsync(User user, Destination dest)
        {
            var similarUsersQuery = _context.Users.AsQueryable();

            if (user.BudgetLevelId.HasValue)
                similarUsersQuery = similarUsersQuery.Where(u => u.BudgetLevelId == user.BudgetLevelId);
            if (user.TransportModeId.HasValue)
                similarUsersQuery = similarUsersQuery.Where(u => u.TransportModeId == user.TransportModeId);
            if (user.PreferredDestinationTypeId.HasValue)
                similarUsersQuery = similarUsersQuery.Where(u => u.PreferredDestinationTypeId == user.PreferredDestinationTypeId);
            if (user.PreferredActivitiesId.HasValue)
                similarUsersQuery = similarUsersQuery.Where(u => u.PreferredActivitiesId == user.PreferredActivitiesId);
            if (user.PreferredAccommodationId.HasValue)
                similarUsersQuery = similarUsersQuery.Where(u => u.PreferredAccommodationId == user.PreferredAccommodationId);
            if (user.CuisineImportanceId.HasValue)
                similarUsersQuery = similarUsersQuery.Where(u => u.CuisineImportanceId == user.CuisineImportanceId);
            if (user.TourismStyleId.HasValue)
                similarUsersQuery = similarUsersQuery.Where(u => u.TourismStyleId == user.TourismStyleId);
            if (user.TripDurationId.HasValue)
                similarUsersQuery = similarUsersQuery.Where(u => u.TripDurationId == user.TripDurationId);
            if (user.TravelGroupId.HasValue)
                similarUsersQuery = similarUsersQuery.Where(u => u.TravelGroupId == user.TravelGroupId);
            if (user.SceneryVibeId.HasValue)
                similarUsersQuery = similarUsersQuery.Where(u => u.SceneryVibeId == user.SceneryVibeId);

            var similarUsers = await similarUsersQuery.Select(u => u.Id).ToListAsync();
            var feedbacks = await _context.UserFeedbacks
                .Where(f => f.DestinationId == dest.Id && similarUsers.Contains(f.UserId))
                .ToListAsync();

            int collaborativeScore = 0;
            foreach (var f in feedbacks)
            {
                if (f.FeedbackId == 1)
                    collaborativeScore += 10;  // Like
                else if (f.FeedbackId == 2)
                    collaborativeScore -= 5;   // Dislike
            }
            return collaborativeScore;
        }
    }
}
