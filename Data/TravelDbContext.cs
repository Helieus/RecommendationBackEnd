using Microsoft.EntityFrameworkCore;
using TravelRecommendationsAPI.Models;

namespace TravelRecommendationsAPI.Data
{
    public class TravelDbContext : DbContext
    {
        public TravelDbContext(DbContextOptions<TravelDbContext> options)
            : base(options)
        {
        }

        // Attribute Tables
        public DbSet<BudgetLevel> BudgetLevels { get; set; }
        public DbSet<TransportMode> TransportModes { get; set; }
        public DbSet<DestinationType> DestinationTypes { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<AccommodationType> AccommodationTypes { get; set; }
        public DbSet<CuisineImportance> CuisineImportance { get; set; }
        public DbSet<TourismStyle> TourismStyles { get; set; }
        public DbSet<TripDuration> TripDurations { get; set; }
        public DbSet<TravelGroup> TravelGroups { get; set; }
        public DbSet<SceneryVibe> SceneryVibes { get; set; }

        // Main Tables
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<User> Users { get; set; }

        // Feedback
        public DbSet<FeedbackType> FeedbackTypes { get; set; }
        public DbSet<UserFeedback> UserFeedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tells EF that 'FeedbackId' is the foreign key for 'FeedbackType'
            modelBuilder.Entity<UserFeedback>()
                .HasOne(uf => uf.FeedbackType)
                .WithMany()  
                .HasForeignKey(uf => uf.FeedbackId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
