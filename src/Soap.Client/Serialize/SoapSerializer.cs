using System.Xml.Serialization;
using Soap.Client.Configuration;
using Soap.Client.Model;

namespace Soap.Client.Serialize;

public class SoapSerializer : ISoapSerializer
{
    private static readonly object Lock = new();
    private static readonly Dictionary<string, XmlSerializer> Cache = new();

    public T DeserializeResponse<T>(SoapVersion soapVersion, string xml) where T : BodyResponse
    {
        var envelopeType = soapVersion == SoapVersion.Soap11 
            ? typeof(EnvelopeResponse11)
            : typeof(EnvelopeResponse12);

        var attributes = new XmlAttributes();
        attributes.XmlElements.Add(new XmlElementAttribute(typeof(T)));

        var attributeOverrides = new XmlAttributeOverrides();
        attributeOverrides.Add(typeof(EnvelopeResponse), "Body", attributes);

        var serializer = GetXmlSerializer<T>(envelopeType, attributeOverrides);

        using var reader = new StringReader(xml);
        var result = serializer.Deserialize(reader);

        if (result is null)
        {
            throw new InvalidOperationException("Failed to deserialize the response");
        }

        var envelope = (EnvelopeResponse) result;
            
        if (envelope.Body is null)
        {
            throw new InvalidOperationException("Body is empty");
        }

        return (T) envelope.Body;
    }

    private static XmlSerializer GetXmlSerializer<T>(Type envelopeType, XmlAttributeOverrides attributeOverrides) where T : BodyResponse
    {
        lock (Lock)
        {
            var key = envelopeType.FullName + typeof(T);

            if (Cache.TryGetValue(key, out var serializer)) return serializer;

            serializer = new XmlSerializer(envelopeType, attributeOverrides);
            Cache.TryAdd(key, serializer);

            return serializer;
        }
    }
}