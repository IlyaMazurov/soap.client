using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Soap.Client.Http.Configuration;
using Soap.Client.Http.DependencyInjection.Infrastructure;
using Soap.Client.Serialize;

[assembly: InternalsVisibleTo("Soap.Client.Http.Logging.DependencyInjection")]

namespace Soap.Client.Http.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSoapHttpClient(this IServiceCollection services, SoapClientConfiguration configuration)
    {
        var builder = services.AddHttpClient<ISoapClient, SoapClient>(client =>
        {
            if (!string.IsNullOrWhiteSpace(configuration.Url)) client.BaseAddress = new Uri(configuration.Url);
            client.Timeout = TimeSpan.FromSeconds(configuration.Timeout);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/soap+xml"));
        });
        
        builder.ConfigurePrimaryHttpMessageHandler(() =>
        {
            var httpClientHandler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            if (configuration.Proxy.Enabled)
            {
                if (string.IsNullOrWhiteSpace(configuration.Proxy.Host))
                {
                    throw new InvalidOperationException("Proxy host can't be empty");
                }
            
                httpClientHandler.Proxy = new WebProxy(configuration.Proxy.Host, configuration.Proxy.Port)
                {
                    Credentials = new NetworkCredential(configuration.Proxy.Username, configuration.Proxy.Password),
                    UseDefaultCredentials = configuration.Proxy.UseDefaultCredentials
                };
            }

            return httpClientHandler;
        });
        
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<ISoapSerializerRegister>? configure = null)
    {
        services.AddDefaultSoapSerializer();
        
        configure?.Invoke(new SoapSerializerRegister(services));

        return services;
    }

    internal static IServiceCollection AddDefaultSoapSerializer(this IServiceCollection services) 
        => services.AddSingleton<ISoapSerializer, SoapSerializer>();
}