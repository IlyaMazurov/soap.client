using Soap.Client.Http.Logging.Context;

namespace Soap.Client.Http.Logging.Logger;

public interface ISoapClientLogger<T> where T : class, ISoapBusinessContext
{
    Task LogAsync(SoapLogContext<T>? context);
}