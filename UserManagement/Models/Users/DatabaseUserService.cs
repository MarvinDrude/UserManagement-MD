
using System.Reflection.Metadata.Ecma335;
using UserManagement.Utilities;

namespace UserManagement.Models.Users;

public sealed class DatabaseUserService(
    DatabaseContext databaseContext, UserManager<DatabaseUser> userManager, IConfiguration configuration) {

    private readonly DatabaseContext DatabaseContext = databaseContext;

    private readonly UserManager<DatabaseUser> UserManager = userManager;

    private readonly IConfiguration Configuration = configuration;

    public async Task<List<DatabaseUser>> GetUsers(string filter, int page, int pageSize) {

        // ignoring sorting by all possible fields in screenshot due to time (better solution: keyset pagination)
        // using naive pagination which will get slower with incrementing pages

        var command = DatabaseContext.Users
            .Where(user => (
                EF.Functions.Like(user.UserName, $"%{filter}%")
                || EF.Functions.Like(user.FirstName, $"%{filter}%")
                || EF.Functions.Like(user.LastName, $"%{filter}%")
                || EF.Functions.Like(user.Position, $"%{filter}%")
                || EF.Functions.Like(user.EmployeeNumber, $"%{filter}%")
            ))
            .OrderBy(user => user.LastName)
            .Skip(page * pageSize)
            .Take(pageSize);

        return await command.ToListAsync();

    }

    public async Task<DatabaseUser?> CreateUser(DatabaseUser user, string password) {

        // user manager manages checks for duplicate email, requirements etc
        // if you want to print custom message for specific errors
        // you have to check it here and return a specific error message for nicer user experience

        var result = await UserManager.CreateAsync(user, password);
        DatabaseContext.Entry(user).State = EntityState.Detached; // entity framework is meh
        
        if(!result.Succeeded) {
            return null;
        }

        DatabaseUser? resultUser = await UserManager.FindByEmailAsync(user.Email!);

        if(resultUser == null) {
            return null;
        }

        await AssignNewInvitationCode(resultUser);
        return resultUser;

    }

    public async Task AssignNewInvitationCode(DatabaseUser user) {

        // here is a very rare race condition of 2 users getting the same invite code 
        // therefore in a real application, to make sure that wont ever happen
        // you should have a unique key on the invitation code, so atleast one runs into an error
        // alternatively you could base the invite code on a specific seeding on unique user properties like id
        // and with that seed you can then create safe readable strings

        string alphabet = Configuration.GetValue<string>("UsersAdmin:InviteCodeGen:Alphabet")!;
        int length = Configuration.GetValue<int>("UsersAdmin:InviteCodeGen:Length")!;

        bool alreadyInUse = false;
        string? code = null;

        do {

            code = RandomUtils.GetRandomReadableString(alphabet, length);
            var command = DatabaseContext.Users.Where(x => x.InvitationCode == code);

            alreadyInUse = await command.AnyAsync();

        } while (alreadyInUse);

        user.InvitationCode = code;

        DatabaseContext.Update(user);
        await DatabaseContext.SaveChangesAsync();

    }

    public async Task<DatabaseUser?> UpdateUser(DatabaseUser user) {

        DatabaseContext.Update(user);
        await DatabaseContext.SaveChangesAsync();

        return await DatabaseContext.Users.Where(x => x.Id == user.Id).FirstOrDefaultAsync();

    }

    public async Task<bool> DeleteUserById(string userId) {

        var user = await DatabaseContext.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        if (user == null) return false;

        DatabaseContext.Remove(user);
        await DatabaseContext.SaveChangesAsync();

        return true;

    }

}
