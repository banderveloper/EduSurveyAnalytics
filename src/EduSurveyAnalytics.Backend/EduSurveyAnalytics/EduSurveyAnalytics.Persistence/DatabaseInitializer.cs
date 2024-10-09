namespace EduSurveyAnalytics.Persistence;

// Invokes in main during server starting
public static class DatabaseInitializer
{
    /// <summary>
    /// Create database if it is not
    /// </summary>
    /// <param name="context">Entity framework database context to create</param>
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
        // Log.Information("Ensured database creating");
    }
}