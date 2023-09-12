using Soap.Client.Http.Logging.Context;
using Soap.Client.Http.Logging.Logger;

namespace Soap.Client.Http.Logging.DependencyInjection.Infrastructure;

public interface ISoapClientLoggerRegister
{
    void AddSoapClientLogger<TSoapClientLogger, TBusinessContext>()
        where TSoapClientLogger : class, ISoapClientLogger<TBusinessContext>
        where TBusinessContext : class, ISoapBusinessContext;
}