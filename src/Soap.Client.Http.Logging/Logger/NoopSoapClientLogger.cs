using Soap.Client.Http.Logging.Context;

namespace Soap.Client.Http.Logging.Logger;

public class NupSoapClientLogger : ISoapClientLogger<NoopSoapBusinessContext>
{
    public Task LogAsync(SoapLogContext<NoopSoapBusinessContext>? context)
    {
        return Task.CompletedTask;
    }
}