using Microsoft.AspNetCore.Http;
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
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Set session ID in a cookie
            Response.Cookies.Append("SessionId", sessionGuid.ToString(), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = false
            });

            return Ok(new { SessionId = sessionGuid });
        }
    }
}
