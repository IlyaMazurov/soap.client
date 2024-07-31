using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Soap.Client.Configuration;
using Soap.Client.Hosting.Extensions;

namespace Soap.Client.Test.Configuration;

public static class ServiceProviderConfigurator
{
    private static readonly string SoapClientConfiguration =
        File.ReadAllText(@"..\..\..\Configuration\SoapClientConfiguration.json", Encoding.UTF8);

    public static IServiceProvider ConfigureHttp() =>
        new ServiceCollection()
            .AddSoapClient(GetSoapClientConfiguration())
            .AddLogging(builder => builder.AddConsole())
            .BuildServiceProvider();

    private static SoapClientConfiguration GetSoapClientConfiguration()
        => JsonConvert.DeserializeObject<SoapClientConfiguration>(SoapClientConfiguration)
           ?? throw new InvalidOperationException(nameof(SoapClientConfiguration));
}
