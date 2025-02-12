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
            // Use the last inserted user record
            var user = await _context.Users.OrderByDescending(u => u.Id).FirstOrDefaultAsync();
            if (user == null)
                return NotFound("No user found.");

            // Check if the provided FeedbackId exists in the FeedbackTypes table
            var feedbackTypeExists = await _context.FeedbackTypes.AnyAsync(ft => ft.Id == feedbackDto.FeedbackId);
            if (!feedbackTypeExists)
            {
                return BadRequest("Invalid FeedbackId provided.");
            }

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
            // Since we are not using sessions now, simply return OK.
            return Ok("Session restarted.");
        }
    }
}
