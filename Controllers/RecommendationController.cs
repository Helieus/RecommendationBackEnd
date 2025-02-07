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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.SessionId == sessionId);

            if (user == null) return NotFound("Session not found.");

            // Map answers to user preferences
            user.BudgetLevel = userAnswers.BudgetLevel;
            user.TransportMode = userAnswers.TransportMode;
            user.PreferredDestinationType = userAnswers.PreferredDestinationType;
            user.PreferredActivities = userAnswers.PreferredActivities;
            user.PreferredAccommodation = userAnswers.PreferredAccommodation;
            user.CuisineImportance = userAnswers.CuisineImportance;
            user.TourismStyle = userAnswers.TourismStyle;
            user.TripDuration = userAnswers.TripDuration;
            user.TravelGroup = userAnswers.TravelGroup;
            user.SceneryVibe = userAnswers.SceneryVibe;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("recommend")]
        public async Task<IActionResult> RecommendDestination()
        {
            var sessionId = Request.Cookies["SessionId"];
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.SessionId == sessionId);

            if (user == null) return NotFound("Session not found.");

            // Get all destinations
            var destinations = await _context.Destinations.ToListAsync();

            // Calculate match scores
            var destinationScores = destinations.Select(d => new
            {
                Destination = d,
                Score = CalculateMatchScore(user, d)
            })
            .OrderByDescending(d => d.Score)
            .ToList();

            // Return top destination
            var topDestination = destinationScores.First().Destination;

            return Ok(topDestination);
        }

        private int CalculateMatchScore(User user, Destination destination)
        {
            int score = 0;

            // Compare each user preference with destination attributes
            if (user.BudgetLevel == destination.BudgetLevel) score += 10;
            if (user.TransportMode == destination.TransportMode) score += 10;
            if (user.PreferredDestinationType == destination.DestinationType) score += 10;
            if (user.PreferredActivities == destination.Activities) score += 10;
            if (user.PreferredAccommodation == destination.AccommodationType) score += 10;
            if (user.CuisineImportance == destination.CuisineImportance) score += 10;
            if (user.TourismStyle == destination.TourismStyle) score += 10;
            if (user.TripDuration == destination.TripDuration) score += 10;
            if (user.TravelGroup == destination.TravelGroup) score += 10;
            if (user.SceneryVibe == destination.SceneryVibe) score += 10;

            return score;
        }
    }
}
