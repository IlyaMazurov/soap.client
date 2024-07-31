using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Xml;
using Soap.Client.Configuration;
using Soap.Client.Models;
using System.Xml.Serialization;

namespace Soap.Client.Serialize;

public class SoapSerializer : ISoapSerializer
{
    private static readonly ConcurrentDictionary<string, XmlSerializer> Cache = new();

    public DeserializeResult<T> TryDeserializeResponse<T>(SoapVersion soapVersion, string xml) where T : BodyResponse
    {
        var result = new DeserializeResult<T>();

        try
        {
            result.Data = DeserializeResponse<T>(soapVersion, xml);
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.ToString();
        }

        return result;
    }

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

        using var stringReader = new StringReader(xml);
        using var xmlReader = XmlReader.Create(stringReader);
        var result = serializer.Deserialize(xmlReader)
                     ?? throw new SerializationException("Failed to deserialize the response");

        var envelope = (EnvelopeResponse)result;

        return envelope.Body is null
            ? throw new SerializationException("Body is empty")
            : (T)envelope.Body;
    }

    private static XmlSerializer GetXmlSerializer<T>(Type envelopeType, XmlAttributeOverrides attributeOverrides) where T : BodyResponse
    {
        var key = envelopeType.FullName + typeof(T);

        if (Cache.TryGetValue(key, out var serializer))
        {
            return serializer;
        }

        serializer = new XmlSerializer(envelopeType, attributeOverrides);
        Cache.TryAdd(key, serializer);

        return serializer;
    }
}
