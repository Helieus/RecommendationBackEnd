using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelRecommendationsAPI.Data;
using TravelRecommendationsAPI.Models;
using TravelRecommendationsAPI.DTOs;

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
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackDto feedback)
        {
            var sessionId = Request.Cookies["SessionId"];
            var user = await _context.Users.FirstOrDefaultAsync(u => u.SessionId == sessionId);

            if (user == null) return NotFound("Session not found.");

            var userFeedback = new UserFeedback
            {
                UserId = user.Id,
                DestinationId = feedback.DestinationId,
                FeedbackId = feedback.FeedbackId
            };

            _context.UserFeedbacks.Add(userFeedback);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("restart")]
        public IActionResult RestartSession()
        {
            // Clear the session cookie
            Response.Cookies.Delete("SessionId");
            return Ok();
        }
    }
}
