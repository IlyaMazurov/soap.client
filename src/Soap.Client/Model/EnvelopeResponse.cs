using System.Xml.Serialization;

namespace Soap.Client.Model;

[XmlInclude(typeof(EnvelopeResponse11))]
[XmlInclude(typeof(EnvelopeResponse12))]
public abstract class EnvelopeResponse
{
    [XmlElement("Body")]
    public BodyResponse? Body;

    protected EnvelopeResponse() { }

    protected EnvelopeResponse(BodyResponse body)
    {
        Body = body;
    }
}

[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class EnvelopeResponse11 : EnvelopeResponse
{
    public EnvelopeResponse11() { }

    public EnvelopeResponse11(BodyResponse body) : base(body) { }
}

[XmlRoot("Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
public class EnvelopeResponse12 : EnvelopeResponse
{
    public EnvelopeResponse12() { }

    public EnvelopeResponse12(BodyResponse body) : base(body) { }
}