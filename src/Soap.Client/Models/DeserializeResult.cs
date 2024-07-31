namespace Soap.Client.Models;

public class DeserializeResult<T> where T : BodyResponse
{
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
}
