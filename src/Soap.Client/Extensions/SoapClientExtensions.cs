using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Soap.Client.Configuration;
using Soap.Client.Model;

namespace Soap.Client.Extensions;

public static class SoapClientExtensions
{
    public static async Task<TResponse> PostAsync<TResponse>(
        this ISoapClient soapClient,
        SoapVersion soapVersion,
        object body,
        object header,
        string? path = null,
        string? action = null) where TResponse : BodyResponse
    {
        var xBody = CreateXElement(body);
        var xHeader = CreateXElement(header);
        
        return await soapClient.PostAsync<TResponse>(soapVersion, new []{xBody}, new []{xHeader}, path, action);
    }
    
    public static async Task<TResponse> PostAsync<TResponse>(
        this ISoapClient soapClient,
        SoapVersion soapVersion,
        object body,
        string? path = null,
        string? action = null) where TResponse : BodyResponse
    {
        var xBody = CreateXElement(body);
        return await soapClient.PostAsync<TResponse>(soapVersion, new []{xBody}, null, path, action);
    }

    public static XElement CreateXElement(object obj)
    {
        var serializer = new XmlSerializer(obj.GetType());
        using var stringWriter = new StringWriter();

        using (var writer = XmlWriter.Create(stringWriter))
        {
            serializer.Serialize(writer, obj);
        }

        var element = XElement.Parse(stringWriter.ToString());

        return element;
    }
}