using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Soap.Client.Extensions;

public static class InstanceExtensions
{
    public static XElement CreateXElement<T>(this T instance) where T : class
    {
        var serializer = new XmlSerializer(instance.GetType());
        using var stringWriter = new StringWriter();

        using (var writer = XmlWriter.Create(stringWriter))
        {
            serializer.Serialize(writer, instance);
        }

        var element = XElement.Parse(stringWriter.ToString());

        return element;
    }
}
