/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelRecommendationsAPI.Data;
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
            var sessionGuid = Guid.NewGuid();
            var user = new User
            {
                SessionId = sessionGuid,
                CreatedAt = DateTime.UtcNow.ToLocalTime()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // For local development over HTTP, use Lax and Secure = false.
            // In production over HTTPS, use SameSite=None and Secure=true.
            Response.Cookies.Append("SessionId", sessionGuid.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to 'true' in production (HTTPS)
                SameSite = SameSiteMode.Lax // Use Lax for local development; in production, you might use None with Secure=true.
            });

            return Ok(new { SessionId = sessionGuid });
        }

        [HttpPost("restart")]
        public async Task<IActionResult> RestartSession()
        {
            // Optionally, remove the old session record or ignore it.
            Response.Cookies.Delete("SessionId");

            // Then start a new session
            return await StartSession();
        }

    }
}*/
