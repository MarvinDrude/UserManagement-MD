
namespace UserManagement.Requests.Users;

/// <summary>
/// Represents the json used to login
/// </summary>
public sealed class SignInRequest {

    public string? UserName { get; set; }

    public string? Password { get; set; }

}

/// <summary>
/// Represents the json response for a login
/// </summary>
public sealed class SignInResponse {

    public bool Success { get; set; }

    public string? Message { get; set; }

}
