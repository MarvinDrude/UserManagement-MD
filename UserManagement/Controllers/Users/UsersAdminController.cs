
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Controllers.Users;

[Route("api/users-admin")]
//[Authorize(Policy = "RequireAdmin")]
[ApiController]
public class UsersAdminController(
    ILogger<UsersAdminController> logger, 
    IConfiguration configuration, 
    DatabaseContext databaseContext, 
    UserManager<DatabaseUser> userManager) : ControllerBase {

    private readonly DatabaseContext DatabaseContext = databaseContext;

    private readonly IConfiguration Configuration = configuration;

    private readonly ILogger<UsersAdminController> Logger = logger;

    private readonly DatabaseUserService UserService = new (databaseContext, userManager, configuration);

    [HttpPost("get-users")]
    public async Task<IActionResult> GetUsers([FromBody]GetUsersRequest request) {

        if(request == null) {

            return BadRequest(new GenericResponse<string>() {
                Success = false,
                Data = "Malformed request."
            });

        }

        // if search null just go ahead and act like its an empty search
        request.Search ??= "";

        int minSize = Configuration.GetValue<int>("UsersAdmin:Pagination:Min");
        int maxSize = Configuration.GetValue<int>("UsersAdmin:Pagination:Max");

        if(request.PageSize < minSize || request.PageSize > maxSize) {

            return BadRequest(new GenericResponse<string>() {
                Success = false,
                Data = "Invalid pagination size."
            });

        }

        // If there is data about users that should be removed before sending to frontend like password hash
        // Somewhere here would be a good place, won't do it now because time and its for admins anyways
        return Ok(await UserService.GetUsers(request.Search, request.Page, request.PageSize));

    }

    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody]CreateUserRequest request) {

        var user = request.User;

        // just checking some required fields and also some that shouldn't be filled
        // lets make sure to stop that just here, more checking here
        // real application would need more checking
        if (user == null || user.UserName == null || user.FirstName == null
            || user.LastName == null || user.Email == null || user.InvitationCode != null
            || request.Password == null) {

            return BadRequest(new GenericResponse<string>() {
                Success = false,
                Data = "Malformed user object."
            });

        }

        var createdUser = await UserService.CreateUser(user, request.Password);

        if (createdUser == null) {

            return BadRequest(new GenericResponse<string>() {
                Success = false,
                Data = "Creation failed." // Would need more specific error handling for better user experience
            });

        }

        return Ok(createdUser);

    }

    [HttpPost("update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] DatabaseUser user) {

        // just checking some required fields and also some that shouldn't be filled
        // real application would need more checking to not override any serious things like invite code etc
        if (user == null || user.UserName == null || user.FirstName == null
            || user.LastName == null || user.Email == null) {

            return BadRequest(new GenericResponse<string>() {
                Success = false,
                Data = "Malformed user object."
            });

        }

        // this update user method can be used to soft delete or soft block users by setting IsDeleted, IsBlocked
        // but also to change all sorts of things
        var resultUser = await UserService.UpdateUser(user);

        return Ok(resultUser);

    }

    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest request) {

        if (request == null || request.UserId == null) {

            return BadRequest(new GenericResponse<string>() {
                Success = false,
                Data = "Malformed user object."
            });

        }

        // this is if you really want to delete a user from the database
        var found = await UserService.DeleteUserById(request.UserId);

        return Ok(new {
            Found = found
        });

    }

}
