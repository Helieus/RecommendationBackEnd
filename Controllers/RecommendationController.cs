using Microsoft.AspNetCore.Http;
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

        [HttpPost("submit-answers")]
        public async Task<IActionResult> SubmitAnswers([FromBody] UserAnswersDto userAnswers)
        {
            var sessionId = Request.Cookies["SessionId"];
            if (!Guid.TryParse(sessionId, out var sessionGuid))
                return BadRequest("Invalid session cookie.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.SessionId == sessionGuid);
            if (user == null) return NotFound("Session not found.");

            // Map answers to user
            user.BudgetLevelId = userAnswers.BudgetLevelId;
            user.TransportModeId = userAnswers.TransportModeId;
            user.PreferredDestinationTypeId = userAnswers.PreferredDestinationTypeId;
            user.PreferredActivitiesId = userAnswers.PreferredActivitiesId;
            user.PreferredAccommodationId = userAnswers.PreferredAccommodationId;
            user.CuisineImportanceId = userAnswers.CuisineImportanceId;
            user.TourismStyleId = userAnswers.TourismStyleId;
            user.TripDurationId = userAnswers.TripDurationId;
            user.TravelGroupId = userAnswers.TravelGroupId;
            user.SceneryVibeId = userAnswers.SceneryVibeId;

            await _context.SaveChangesAsync();
            return Ok("Answers submitted successfully.");
        }

        [HttpGet("recommend")]
        public async Task<IActionResult> RecommendDestination()
        {
            var sessionId = Request.Cookies["SessionId"];
            if (!Guid.TryParse(sessionId, out var sessionGuid))
                return BadRequest("Invalid session cookie.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.SessionId == sessionGuid);
            if (user == null) return NotFound("Session not found.");

            // 1) Content-based approach: For each destination, count how many preferences match
            var allDestinations = await _context.Destinations.ToListAsync();

            // Build a list of { Destination, ContentScore, CollaborativeScore }
            var scoredDests = new List<(Destination Destination, int ContentScore, int CollaborativeScore)>();

            foreach (var dest in allDestinations)
            {
                int contentScore = CalculateContentScore(user, dest);
                int collaborativeScore = await CalculateCollaborativeScoreAsync(user, dest);

                scoredDests.Add((dest, contentScore, collaborativeScore));
            }

            // Sort by total (content + collaborative) desc
            var best = scoredDests
                .Select(s => new
                {
                    Destination = s.Destination,
                    TotalScore = s.ContentScore + s.CollaborativeScore
                })
                .OrderByDescending(x => x.TotalScore)
                .FirstOrDefault();

            if (best == null) return Ok("No destinations found or scored.");

            // Return top destination
            return Ok(best.Destination);
        }

        private int CalculateContentScore(User user, Destination dest)
        {
            int score = 0;
            // For each matching preference, add e.g. 10 points
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

        private async Task<int> CalculateCollaborativeScoreAsync(User user, Destination dest)
        {
            // 2) Collaborative approach: 
            // Find other users with same preferences
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

            // Summarize feedback from these similar users on this destination
            // e.g., if FeedbackId=1 => +10, if FeedbackId=2 => -5, else 0
            var feedbacks = await _context.UserFeedbacks
                .Where(f => f.DestinationId == dest.Id && similarUsers.Contains(f.UserId))
                .ToListAsync();

            int collaborativeScore = 0;
            foreach (var f in feedbacks)
            {
                if (f.FeedbackId == 1) collaborativeScore += 10;  // Like
                else if (f.FeedbackId == 2) collaborativeScore -= 5;  // Dislike
                // Adjust scoring logic as you see fit
            }

            return collaborativeScore;
        }
    }
}
