using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Soap.Client.Http.DependencyInjection;
using Soap.Client.Http.Configuration;
using Soap.Client.Http.DependencyInjection.Infrastructure;

namespace Soap.Client.Http.Hosting;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseSoapHttpClient(this IHostBuilder builder, Action<ISoapSerializerRegister>? registerSerializer = null)
    {
        builder.ConfigureServices((context, services) =>
        {
            var configuration = GetSoapClientConfiguration(context.Configuration);
            services.AddSoapHttpClient(configuration);
            services.AddInfrastructure(registerSerializer);
        });

        return builder;
    }

    private static SoapClientConfiguration GetSoapClientConfiguration(IConfiguration configuration, string key = "soapclientconfiguration")
    {
        var soapClientConfigurationSection = configuration.GetSection(key);
        
        var soapClientConfiguration = soapClientConfigurationSection.Exists() ? soapClientConfigurationSection.Get<SoapClientConfiguration>() : null;
        if (soapClientConfiguration is null)
        {
            throw new InvalidOperationException(nameof(soapClientConfiguration));
        }

        return soapClientConfiguration;
    }
}