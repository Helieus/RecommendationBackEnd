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
    public class FeedbackController : ControllerBase
    {
        private readonly TravelDbContext _context;

        public FeedbackController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackDto feedbackDto)
        {
            var sessionId = Request.Cookies["SessionId"];
            if (!Guid.TryParse(sessionId, out var sessionGuid))
                return BadRequest("Invalid session cookie.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.SessionId == sessionGuid);
            if (user == null) return NotFound("Session not found.");

            var userFeedback = new UserFeedback
            {
                UserId = user.Id,
                DestinationId = feedbackDto.DestinationId,
                FeedbackId = feedbackDto.FeedbackId,
                Timestamp = DateTime.UtcNow
            };

            _context.UserFeedbacks.Add(userFeedback);
            await _context.SaveChangesAsync();

            return Ok("Feedback submitted successfully.");
        }

        [HttpPost("restart")]
        public IActionResult RestartSession()
        {
            // Clear the session cookie
            Response.Cookies.Delete("SessionId");
            return Ok("Session restarted; cookie cleared.");
        }
    }
}
