using Soap.Client.Serialize;

namespace Soap.Client.Http.DependencyInjection.Infrastructure;

public interface ISoapSerializerRegister
{
    void AddSoapSerializer<T>() where T : class, ISoapSerializer;
}

