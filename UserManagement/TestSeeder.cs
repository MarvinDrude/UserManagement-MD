
namespace UserManagement;

public sealed class TestSeeder(UserManager<DatabaseUser> userManager, RoleManager<IdentityRole> roleManager) {

    private readonly UserManager<DatabaseUser> UserManager = userManager;

    private readonly RoleManager<IdentityRole> RoleManager = roleManager;

    /// <summary>
    /// Quick and dirty way to seed users and roles for this example project
    /// </summary>
    /// <returns></returns>
    public async Task Seed() {

        string[] roleNames = ["Admin", "User"];

        foreach (var roleName in roleNames) {

            if (!await RoleManager.RoleExistsAsync(roleName)) {
                await RoleManager.CreateAsync(new IdentityRole(roleName));
            }

        }

        DatabaseUser? user = await UserManager.FindByNameAsync("admin-account");

        if (user == null) {

            user = new DatabaseUser() {
                UserName = "admin-account",
                Email = "admin-account@test.com",
                FirstName = "Marvin",
                LastName = "Drude",
            };
            var resp = await UserManager.CreateAsync(user, "Testpassword1!");

            user = await UserManager.FindByNameAsync("admin-account");
            await UserManager.AddToRolesAsync(user!, ["Admin"]);

        }

    }

}