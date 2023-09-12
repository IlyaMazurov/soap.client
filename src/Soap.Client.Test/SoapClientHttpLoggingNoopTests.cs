using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Soap.Client.Configuration;
using Soap.Client.Extensions;
using Soap.Client.Http.Logging.Context;
using Soap.Client.Http.Logging.Logger;
using Soap.Client.Test.Configuration;
using Soap.Client.Test.Models;

namespace Soap.Client.Test;

[TestFixture]
public class SoapClientHttpLoggingNoopTests
{
    private readonly IServiceProvider _serviceProvider = ServiceProviderConfigurator.ConfigureDefaultHttpLogging();
    
    [Test]
    public void CheckRegisterNoopLogger()
    {
        var logger = _serviceProvider.GetRequiredService<ISoapClientLogger<NoopSoapBusinessContext>>();
        
        Assert.That(logger, Is.Not.Null);
    }
    
    [Test]
    public async Task SendRequestTest()
    {
        var client = _serviceProvider.GetRequiredService<ISoapClient>();
        var response = await client.PostAsync<AuthmethodResponseBody>(SoapVersion.Soap12, new AuthmethodRequest());
        
        Assert.That(response.AuthmethodResponse?.AuthmethodResult, Is.EqualTo("True"));
    }
    
    [Test]
    public async Task CreateSoapLogAmbientTest()
    {
        using (SoapLogAmbient<NoopSoapBusinessContext>.Init(new NoopSoapBusinessContext()))
        {
            var client = _serviceProvider.GetRequiredService<ISoapClient>();
            await client.PostAsync<AuthmethodResponseBody>(SoapVersion.Soap12, new AuthmethodRequest());

            Assert.That(SoapLogAmbient<NoopSoapBusinessContext>.GetContext(), Is.Not.Null);
        }
    }
}