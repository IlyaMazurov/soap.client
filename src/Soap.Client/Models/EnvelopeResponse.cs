using System.Xml.Serialization;

namespace Soap.Client.Models;

[XmlInclude(typeof(EnvelopeResponse11))]
[XmlInclude(typeof(EnvelopeResponse12))]
public abstract class EnvelopeResponse
{
    [XmlElement("Body")]
    public BodyResponse? Body { get; set; }
}

[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class EnvelopeResponse11 : EnvelopeResponse
{
}

[XmlRoot("Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
public class EnvelopeResponse12 : EnvelopeResponse
{
}
