namespace UserManagement.Requests.Users;

public sealed class CreateUserRequest {

    public DatabaseUser? User { get; set; }

    public string? Password { get; set; }

}
