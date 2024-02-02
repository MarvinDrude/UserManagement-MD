
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace UserManagement.Identity;

/// <summary>
/// Used to block sign ins from blocked or deleted users
/// </summary>
public class DatabaseSignInManager(
    UserManager<DatabaseUser> userManager,
    IHttpContextAccessor contextAccessor,
    IUserClaimsPrincipalFactory<DatabaseUser> claimsFactory,
    IOptions<IdentityOptions> optionsAccessor,
    ILogger<SignInManager<DatabaseUser>> logger,
    IAuthenticationSchemeProvider schemes,
    IUserConfirmation<DatabaseUser> confirmation) 

    : SignInManager<DatabaseUser>(
        userManager, 
        contextAccessor, 
        claimsFactory, 
        optionsAccessor, 
        logger, 
        schemes, 
        confirmation) {

    /// <summary>
    /// Used as check to whether a user can sign in or not
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public override async Task<bool> CanSignInAsync(DatabaseUser user) {

        if(Options.SignIn.RequireConfirmedEmail && !(await UserManager.IsEmailConfirmedAsync(user))) {

            Logger.LogInformation("{Id} couldn't sign in because email not confirmed.", user.Id);
            return false;

        }

        if(user.UserName == null) {
            return false;
        }

        var fullUser = await UserManager.FindByNameAsync(user.UserName);

        if (fullUser == null || fullUser.IsBlocked || fullUser.IsDeleted) {

            Logger.LogInformation("{Id} couldn't sign in because its blocked or deleted.", user.Id);
            return false;

        }

        return true;

    }

}
