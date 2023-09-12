namespace Soap.Client.Http.Configuration;

public class SoapClientConfiguration
{
    public string? Url { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int Timeout { get; set; }

    public ProxyConfiguration Proxy { get; set; } = new();
}

public class ProxyConfiguration
{
    public bool Enabled { get; set; }

    public string? Host { get; set; }

    public int Port { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public bool UseDefaultCredentials { get; set; }
}