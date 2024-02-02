
namespace UserManagement.Controllers.Users;

/// <summary>
/// Sign in of a user, in this example project just for me to test
/// </summary>
[Route("api/users")]
[ApiController]
public class UsersController(SignInManager<DatabaseUser> signInManager, UserManager<DatabaseUser> userManager) : ControllerBase {

    private readonly SignInManager<DatabaseUser> SignInManager = signInManager;

    private readonly UserManager<DatabaseUser> UserManager = userManager;

    /// <summary>
    /// Sign ins a user by providing username and password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request) {

        var response = new SignInResponse() {
            Message = "Successfully signed in.",
            Success = false
        };

        if(HttpContext.User != null && HttpContext.User.Identity != null
            && HttpContext.User.Identity.IsAuthenticated) {

            response.Message = "Already signed in.";
            return BadRequest(response);

        }

        if(request == null || request.UserName == null || request.Password == null) {

            response.Message = "Invalid request body.";
            return BadRequest(response);

        }

        var result = await SignInManager.PasswordSignInAsync(request.UserName, request.Password, true, false);

        if(!result.Succeeded) {

            response.Message = "Sign in failed.";
            return Ok(response);

        }

        response.Success = true;
        return Ok(response);

    }

    /// <summary>
    /// Sign out a user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("signout")]
    public async Task<IActionResult> LogOut() {

        if (HttpContext.User != null && HttpContext.User.Identity != null
            && HttpContext.User.Identity.IsAuthenticated) {

            await SignInManager.SignOutAsync();

        }

        return Ok();

    }

}
