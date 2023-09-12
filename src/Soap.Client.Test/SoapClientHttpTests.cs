using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Soap.Client.Configuration;
using Soap.Client.Http.Context;
using Soap.Client.Test.Configuration;
using Soap.Client.Extensions;
using Soap.Client.Test.Models;

namespace Soap.Client.Test;

[TestFixture]
public class SoapClientHttpTests
{
    private readonly IServiceProvider _serviceProvider = ServiceProviderConfigurator.ConfigureHttp();

    [Test]
    public async Task SendRequestTest()
    {
        var client = _serviceProvider.GetRequiredService<ISoapClient>();
        var response = await client.PostAsync<AuthmethodResponseBody>(SoapVersion.Soap12, new AuthmethodRequest());
        
        Assert.That(response.AuthmethodResponse?.AuthmethodResult, Is.EqualTo("True"));
    }

    [Test]
    public async Task CreateSoapClientAmbientTest()
    {
        using (SoapClientAmbient.Init())
        {
            var client = _serviceProvider.GetRequiredService<ISoapClient>();
            await client.PostAsync<AuthmethodResponseBody>(SoapVersion.Soap12, new AuthmethodRequest());

            Assert.That(SoapClientAmbient.GetContext(), Is.Not.Null);
        }
    }

    [Test]
    public async Task CheckScopeSoapClientAmbientTest()
    {
        using (SoapClientAmbient.Init())
        {
            var client = _serviceProvider.GetRequiredService<ISoapClient>();
            await client.PostAsync<AuthmethodResponseBody>(SoapVersion.Soap12, new AuthmethodRequest());
        }

        Assert.Throws<InvalidOperationException>(() => SoapClientAmbient.GetContext());
    }
}