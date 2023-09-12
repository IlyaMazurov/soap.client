using Microsoft.Extensions.DependencyInjection;
using Soap.Client.Http.Logging.Context;
using Soap.Client.Http.Logging.Logger;

namespace Soap.Client.Http.Logging.DependencyInjection.Infrastructure;

public class SoapClientLoggerRegister : ISoapClientLoggerRegister
{
    private readonly IServiceCollection _services;
    public SoapClientLoggerRegister(IServiceCollection services) => _services = services;
    
    public void AddSoapClientLogger<TSoapClientLogger, TBusinessContext>()
        where TSoapClientLogger : class, ISoapClientLogger<TBusinessContext>
        where TBusinessContext : class, ISoapBusinessContext
    {
        _services.DecorateSoapClient<TBusinessContext>();
        _services.AddTransient<ISoapClientLogger<TBusinessContext>, TSoapClientLogger>();
    }
}
