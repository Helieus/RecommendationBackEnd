using Microsoft.EntityFrameworkCore;
using TravelRecommendationsAPI.Models;

namespace TravelRecommendationsAPI.Data
{
    public class TravelDbContext : DbContext
    {
        public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options) { }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FeedbackType> FeedbackTypes { get; set; }
        public DbSet<UserFeedback> UserFeedbacks { get; set; }
    }
}