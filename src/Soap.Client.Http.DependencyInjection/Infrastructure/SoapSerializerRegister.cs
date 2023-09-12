using Microsoft.Extensions.DependencyInjection;
using Soap.Client.Serialize;

namespace Soap.Client.Http.DependencyInjection.Infrastructure;

public class SoapSerializerRegister : ISoapSerializerRegister
{
    private readonly IServiceCollection _services;

    public SoapSerializerRegister(IServiceCollection services) => _services = services;

    public void AddSoapSerializer<T>() where T : class, ISoapSerializer
    {
        _services.AddSingleton<ISoapSerializer, T>();
    }
}