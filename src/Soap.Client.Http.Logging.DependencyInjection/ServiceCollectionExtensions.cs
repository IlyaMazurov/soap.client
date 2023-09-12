using Microsoft.Extensions.DependencyInjection;
using Soap.Client.Http.Logging.Context;
using Soap.Client.Http.Logging.DependencyInjection.Infrastructure;
using Soap.Client.Http.Logging.Logger;

namespace Soap.Client.Http.Logging.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<ISoapClientLoggerRegister>? registerLogger = null)
    {
        if (registerLogger is not null)
        {
            registerLogger.Invoke(new SoapClientLoggerRegister(services));
        }
        else
        {
            services
                .AddDefaultLogger()
                .DecorateDefaultSoapClient();
        }

        return services;
    }
    
    public static IServiceCollection DecorateSoapClient<T>(this IServiceCollection services) where T : class, ISoapBusinessContext 
        => services.Decorate<ISoapClient, SoapClientDecorator<T>>();
    
    private static IServiceCollection DecorateDefaultSoapClient(this IServiceCollection services)
        => services.DecorateSoapClient<NoopSoapBusinessContext>();

    private static IServiceCollection AddDefaultLogger(this IServiceCollection services) 
        => services.AddTransient<ISoapClientLogger<NoopSoapBusinessContext>, NupSoapClientLogger>();
}