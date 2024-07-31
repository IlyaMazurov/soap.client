using Soap.Client.Serialize;

namespace Soap.Client.Hosting.Register;

public interface ISoapSerializerRegister
{
    void AddSoapSerializer<T>() where T : class, ISoapSerializer;
}

