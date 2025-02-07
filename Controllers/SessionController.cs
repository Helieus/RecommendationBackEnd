using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelRecommendationsAPI.Data;
using TravelRecommendationsAPI.DTOs;
using TravelRecommendationsAPI.Models;

namespace TravelRecommendationsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly TravelDbContext _context;

        public SessionController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartSession()
        {
            var sessionId = Guid.NewGuid().ToString();
            var user = new User {
                SessionId = sessionId,
                BudgetLevel = "default", // or any appropriate default value
                TransportMode = "default",
                PreferredDestinationType = "default",
                PreferredActivities = "default",
                PreferredAccommodation = "default",
                CuisineImportance = "default",
                TourismStyle = "default",
                TripDuration = "default",
                TravelGroup = "default",
                SceneryVibe = "default"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Set session ID in a cookie
            Response.Cookies.Append("SessionId", sessionId, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = false
            });

            return Ok(new { SessionId = sessionId });
        }
    }
}
