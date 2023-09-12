using System.Xml.Serialization;
using Soap.Client.Model;

namespace Soap.Client.Test.Models;

public class AuthmethodResponseBody : BodyResponse
{
    [XmlElement(Namespace = "http://interfax.ru/ifax")]
    public AuthmethodResponse? AuthmethodResponse { get; set; }
}

public class AuthmethodResponse
{
    [XmlElement] 
    public string? AuthmethodResult { get; set; }
}
