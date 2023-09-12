using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Soap.Client.Http.Configuration;
using Soap.Client.Test.Logger;

namespace Soap.Client.Test.Configuration;

public static class ServiceProviderConfigurator
{
    private static readonly string SoapClientConfiguration = File.ReadAllText(@"..\..\..\Configuration\SoapClientConfiguration.json", Encoding.UTF8);
    
    public static IServiceProvider ConfigureHttp()
    {
        var services = new ServiceCollection();

        Http.DependencyInjection.ServiceCollectionExtensions.AddSoapHttpClient(services, GetSoapClientConfiguration());
        Http.DependencyInjection.ServiceCollectionExtensions.AddInfrastructure(services);

        services.AddLogging(builder => builder.AddConsole());

        return services.BuildServiceProvider();
    }
    
    public static IServiceProvider ConfigureHttpLogging()
    {
        var services = new ServiceCollection();
        
        Http.DependencyInjection.ServiceCollectionExtensions.AddSoapHttpClient(services, GetSoapClientConfiguration());
        Http.DependencyInjection.ServiceCollectionExtensions.AddInfrastructure(services);
        Http.Logging.DependencyInjection.ServiceCollectionExtensions.AddInfrastructure(services, 
            register => register.AddSoapClientLogger<CustomSoapClientLogger, CustomSoapBusinessContext>());

        services.AddLogging(builder => builder.AddConsole());

        return services.BuildServiceProvider();
    }

    public static IServiceProvider ConfigureDefaultHttpLogging()
    {
        var services = new ServiceCollection();
        
        Http.DependencyInjection.ServiceCollectionExtensions.AddSoapHttpClient(services, GetSoapClientConfiguration());
        Http.DependencyInjection.ServiceCollectionExtensions.AddInfrastructure(services);
        Http.Logging.DependencyInjection.ServiceCollectionExtensions.AddInfrastructure(services);

        services.AddLogging(builder => builder.AddConsole());

        return services.BuildServiceProvider();
    }

    private static SoapClientConfiguration GetSoapClientConfiguration()
        => JsonConvert.DeserializeObject<SoapClientConfiguration>(SoapClientConfiguration)
           ?? throw new InvalidOperationException(nameof(SoapClientConfiguration));
}