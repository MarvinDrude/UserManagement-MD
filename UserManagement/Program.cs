
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthorization(options => {
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddScoped<SignInManager<DatabaseUser>, DatabaseSignInManager>();

builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddIdentity<DatabaseUser, IdentityRole>(
    options => {

        options.User.RequireUniqueEmail = true;

    })
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddRoles<IdentityRole>()
    .AddSignInManager<DatabaseSignInManager>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// just for testing purposes
using (var scope = app.Services.CreateScope()) {

    var context = scope.ServiceProvider.GetService<DatabaseContext>()!;

    // disable cache files for this example project
    await context.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=DELETE;");

    var seeder = new TestSeeder(
        scope.ServiceProvider.GetService<UserManager<DatabaseUser>>()!,
        scope.ServiceProvider.GetService<RoleManager<IdentityRole>>()!);

    // create roles and user
    await seeder.Seed();

}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
