using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Soap.Client.Extensions;
using Soap.Client.Configuration;
using Soap.Client.Test.Models;
using Soap.Client.Test.Configuration;

namespace Soap.Client.Test;

[TestFixture]
public class SoapClientHttpTests
{
    private readonly IServiceProvider _serviceProvider = ServiceProviderConfigurator.ConfigureHttp();

    [Test]
    public async Task SendRequestTest()
    {
        var client = _serviceProvider.GetRequiredService<ISoapClient>();
        var response = await client.PostAsync<AuthmethodResponseBody>(SoapVersion.Soap12, new AuthmethodRequest { Test = "test" });

        Assert.That(response.Data?.AuthmethodResponse?.AuthmethodResult, Is.EqualTo("True"));
    }
}
