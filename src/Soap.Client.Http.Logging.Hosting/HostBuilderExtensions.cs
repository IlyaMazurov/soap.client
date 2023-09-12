using Microsoft.Extensions.Hosting;
using Soap.Client.Http.DependencyInjection.Infrastructure;
using Soap.Client.Http.Logging.DependencyInjection;
using Soap.Client.Http.Logging.DependencyInjection.Infrastructure;
using SoapClientHttpBuilder = Soap.Client.Http.Hosting.HostBuilderExtensions;

namespace Soap.Client.Http.Logging.Hosting;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseSoapHttpClient(this IHostBuilder builder,
        Action<ISoapClientLoggerRegister>? registerLogger = null,
        Action<ISoapSerializerRegister>? registerSerializer = null)
    {
        return SoapClientHttpBuilder
            .UseSoapHttpClient(builder, registerSerializer)
            .ConfigureServices((_, services) => services.AddInfrastructure(registerLogger));
    }
}