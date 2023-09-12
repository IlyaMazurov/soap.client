using Soap.Client.Http.Logging.Context;

namespace Soap.Client.Test.Logger;

public class CustomSoapBusinessContext : ISoapBusinessContext
{
    public string? Name { get; set; } = "test";

    public string? Result { get; set; }
}