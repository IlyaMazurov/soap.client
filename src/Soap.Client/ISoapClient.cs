using System.Xml.Linq;
using Soap.Client.Configuration;
using Soap.Client.Model;

namespace Soap.Client;

public interface ISoapClient
{
    Task<TResponse> PostAsync<TResponse>(
        SoapVersion soapVersion,
        IEnumerable<XElement> bodies,
        IEnumerable<XElement>? headers = null,
        string? path = null, 
        string? action = null) where TResponse : BodyResponse;
}