
namespace UserManagement.Requests.Users;

public sealed class GetUsersRequest {

    public string? Search { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

}
