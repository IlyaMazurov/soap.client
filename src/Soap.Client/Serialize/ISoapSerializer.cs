using Soap.Client.Configuration;
using Soap.Client.Model;

namespace Soap.Client.Serialize;

public interface ISoapSerializer
{
    T DeserializeResponse<T>(SoapVersion soapVersion, string xml) where T : BodyResponse;
}