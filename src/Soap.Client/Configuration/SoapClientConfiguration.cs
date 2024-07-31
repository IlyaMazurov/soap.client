namespace Soap.Client.Configuration;

public class SoapClientConfiguration
{
    public Uri Url { get; init; } = null!;
    public int Timeout { get; init; }
    public ProxyConfiguration? Proxy { get; init; }
    public BasicAuthentication? Authentication { get; set; }
    public bool LoggingEnabled { get; init; } = true;
}

public class ProxyConfiguration
{
    public bool Enabled { get; init; }
    public string Host { get; init; } = null!;
    public int Port { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    public bool UseDefaultCredentials { get; init; }
}

public class BasicAuthentication
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Enabled { get; set; }
}
