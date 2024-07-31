using System.Xml.Linq;

namespace Soap.Client.Configuration;

public class SoapMessageConfiguration
{
    public SoapVersion SoapVersion { get; }
    public string MediaType { get; }
    public XNamespace Schema { get; }

    public SoapMessageConfiguration(SoapVersion soapVersion)
    {
        SoapVersion = soapVersion;

        MediaType = soapVersion == SoapVersion.Soap11
            ? "text/xml"
            : "application/soap+xml";

        Schema = soapVersion == SoapVersion.Soap11
            ? "http://schemas.xmlsoap.org/soap/envelope/"
            : "http://www.w3.org/2003/05/soap-envelope";
    }
}
