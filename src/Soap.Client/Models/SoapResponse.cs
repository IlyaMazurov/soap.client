using System.Net;
using System.Text.Json;

namespace Soap.Client.Models;

public abstract class SoapResponse
{
    public Uri Url { get; init; } = null!;
    public string Request { get; set; } = null!;
    public string? Content { get; set; }

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                Success = false;
            }
        }
    }

    public HttpStatusCode StatusCode { get; set; }

    public bool Success { get; set; } = true;
}

public class SoapResponse<T> : SoapResponse where T : BodyResponse
{
    public T? Data { get; set; }

    //todo: убрать после добавления символа @ в LoggerMessageAttribute
    public override string ToString() => JsonSerializer.Serialize(this, JsonSerializerOptionsCache.Options);
}

public static class JsonSerializerOptionsCache
{
    public static JsonSerializerOptions Options { get; } = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
    };
}
