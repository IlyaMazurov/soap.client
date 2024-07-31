using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Soap.Client.Http;
using Soap.Client.Serialize;
using Soap.Client.Configuration;
using Soap.Client.Decorate;
using Soap.Client.Hosting.Register;

[assembly: InternalsVisibleTo("Soap.Client.Test")]

namespace Soap.Client.Hosting.Extensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSoapClient(this IServiceCollection services,
        SoapClientConfiguration configuration,
        Action<ISoapSerializerRegister>? configure = null)
        => services
            .AddSoapHttpClient(configuration)
            .AddLogging(configuration)
            .AddSoapSerializer(configure);

    private static IServiceCollection AddLogging(this IServiceCollection services, SoapClientConfiguration configuration)
        => configuration.LoggingEnabled
            ? services.Decorate<ISoapClient, SoapClientLoggingDecorator>()
            : services;

    private static IServiceCollection AddSoapHttpClient(this IServiceCollection services, SoapClientConfiguration configuration)
    {
        var builder = services.AddHttpClient<ISoapClient, SoapClient>(client =>
        {
            client.BaseAddress = configuration.Url;
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

            if (configuration.Authentication is not null && configuration.Authentication.Enabled)
            {
                httpClientHandler.Credentials = new NetworkCredential(configuration.Authentication.Username, configuration.Authentication.Password);
            }

            if (configuration.Proxy is not null && configuration.Proxy.Enabled)
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

    private static IServiceCollection AddSoapSerializer(this IServiceCollection services, Action<ISoapSerializerRegister>? configure = null)
    {
        services.AddSingleton<ISoapSerializer, SoapSerializer>();

        configure?.Invoke(new SoapSerializerRegister(services));

        return services;
    }
}
