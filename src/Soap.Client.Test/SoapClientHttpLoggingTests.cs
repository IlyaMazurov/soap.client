using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Soap.Client.Configuration;
using Soap.Client.Http.Logging.Logger;
using Soap.Client.Extensions;
using Soap.Client.Http.Logging.Context;
using Soap.Client.Test.Configuration;
using Soap.Client.Test.Logger;
using Soap.Client.Test.Models;

namespace Soap.Client.Test;

public class SoapClientHttpLoggingTests
{
    private readonly IServiceProvider _serviceProvider = ServiceProviderConfigurator.ConfigureHttpLogging();

    [Test]
    public void CheckRegisterLogger()
    {
        var logger = _serviceProvider.GetRequiredService<ISoapClientLogger<CustomSoapBusinessContext>>();

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
        using (SoapLogAmbient<CustomSoapBusinessContext>.Init(new CustomSoapBusinessContext()))
        {
            var client = _serviceProvider.GetRequiredService<ISoapClient>();
            await client.PostAsync<AuthmethodResponseBody>(SoapVersion.Soap12, new AuthmethodRequest());

            Assert.That(SoapLogAmbient<CustomSoapBusinessContext>.GetContext(), Is.Not.Null);
        }
    }

    [Test]
    public void CheckWorkWithMultiplyUseClientTest()
    {
        async Task Action()
        {
            var businessContext = new CustomSoapBusinessContext {Name = "testtest"};
            
            using (SoapLogAmbient<CustomSoapBusinessContext>.Init(businessContext))
            {
                var client = _serviceProvider.GetRequiredService<ISoapClient>();
                var response = await client.PostAsync<AuthmethodResponseBody>(SoapVersion.Soap12, new AuthmethodRequest());

                businessContext.Result = response.AuthmethodResponse?.AuthmethodResult;
            }
        }

        Task.WaitAll(Action(), Action(), Action());
    }

    [Test]
    public async Task CheckDisposeSoapLogAmbientTest()
    {
        using (SoapLogAmbient<CustomSoapBusinessContext>.Init(new CustomSoapBusinessContext()))
        {
            var client = _serviceProvider.GetRequiredService<ISoapClient>();
            await client.PostAsync<AuthmethodResponseBody>(SoapVersion.Soap12, new AuthmethodRequest());
        }
        
        Assert.That(SoapLogAmbient<CustomSoapBusinessContext>.GetContext(), Is.Null);
    }
}