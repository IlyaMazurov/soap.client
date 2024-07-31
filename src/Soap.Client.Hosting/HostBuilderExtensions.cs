using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Soap.Client.Configuration;
using Soap.Client.Hosting.Extensions;
using Soap.Client.Hosting.Register;

namespace Soap.Client.Hosting;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseSoapClient(this IHostBuilder builder, Action<ISoapSerializerRegister>? registerSerializer = null)
        => builder.ConfigureServices((context, services) =>
        {
            var configuration = GetSoapClientConfiguration(context.Configuration);
            services.AddSoapClient(configuration);
        });

    private static SoapClientConfiguration GetSoapClientConfiguration(IConfiguration configuration)
        => configuration.GetSection(nameof(SoapClientConfiguration)).Get<SoapClientConfiguration>()
           ?? throw new InvalidOperationException($"Configuration section not found {nameof(SoapClientConfiguration)}");
}
