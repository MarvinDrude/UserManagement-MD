
namespace UserManagement.Identity;

/// <summary>
/// Represents the database context using a primary constructor
/// </summary>
/// <param name="options"></param>
public sealed class DatabaseContext(
    IConfiguration configuration, DbContextOptions<DatabaseContext> options) : IdentityDbContext<DatabaseUser>(options) {

    private readonly IConfiguration Configuration = configuration;

    private readonly string ConnectionStringKey = "WebApiConnection";

    protected override void OnConfiguring(DbContextOptionsBuilder options) {

        // Using sql lite in order for anyone to run it with vs without needing a specific db running
        options.UseSqlite(Configuration.GetConnectionString(ConnectionStringKey))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    }

}
