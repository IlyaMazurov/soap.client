namespace Soap.Client.Extensions;

public static class HttpClientExtensions
{
    public static Uri TryGetBaseAddress(this HttpClient client) =>
        client.BaseAddress ?? throw new InvalidOperationException();
}
