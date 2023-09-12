using System.Xml.Linq;
using Soap.Client.Configuration;

namespace Soap.Client.Http.Configuration;

public record SoapMessageConfiguration(SoapVersion SoapVersion)
{
    public string MediaType =>
        SoapVersion == SoapVersion.Soap11
            ? "text/xml"
            : "application/soap+xml";

    public XNamespace Schema =>
        SoapVersion == SoapVersion.Soap11
            ? "http://schemas.xmlsoap.org/soap/envelope/"
            : "http://www.w3.org/2003/05/soap-envelope";
}