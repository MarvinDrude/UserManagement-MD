
namespace UserManagement.Models.Users;

/// <summary>
/// Represents a user that can interact with the system
/// </summary>
public sealed class DatabaseUser : IdentityUser {

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Position { get; set; }

    public string? EmployeeNumber { get; set; }

    public string? IdNumber { get; set; }

    public string? InvitationCode { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsBlocked { get; set; }

}
