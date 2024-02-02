
namespace UserManagement.Requests;

public sealed class GenericResponse<T> {

    public bool Success { get; set; }

    public T? Data { get; set; }

}
