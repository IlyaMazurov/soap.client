using Soap.Client.Configuration;
using Soap.Client.Models;

namespace Soap.Client.Serialize;

public interface ISoapSerializer
{
    T DeserializeResponse<T>(SoapVersion soapVersion, string xml) where T : BodyResponse;

    DeserializeResult<T> TryDeserializeResponse<T>(SoapVersion soapVersion, string xml) where T : BodyResponse;
}
