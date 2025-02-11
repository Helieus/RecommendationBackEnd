using Microsoft.EntityFrameworkCore;
using TravelRecommendationsAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Get allowed origins from configuration
var allowedOrigins = builder.Configuration
    .GetSection("CorsSettings:AllowedOrigins")
    .Get<string[]>();

// Add CORS services and configure the policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Keep lazy loading for convenience
builder.Services.AddDbContext<TravelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseLazyLoadingProxies()
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS middleware using the defined policy
app.UseCors("CorsPolicy");

// Uncomment if needed: app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
