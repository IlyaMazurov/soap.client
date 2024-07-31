using System.Text;
using System.Xml.Linq;
using System.Net.Http.Headers;
using Soap.Client.Serialize;
using Soap.Client.Configuration;
using Soap.Client.Extensions;
using Soap.Client.Models;

namespace Soap.Client.Http;

public class SoapClient : ISoapClient
{
    private readonly HttpClient _client;
    private readonly ISoapSerializer _soapSerializer;

    public SoapClient(
        HttpClient client,
        ISoapSerializer soapSerializer)
    {
        _client = client;
        _soapSerializer = soapSerializer;
    }

    public async Task<SoapResponse<TResponse>> PostAsync<TResponse>(
        SoapVersion soapVersion,
        IEnumerable<XElement> bodies,
        IEnumerable<XElement>? headers = null,
        string? path = null,
        string? action = null) where TResponse : BodyResponse
    {
        var response = new SoapResponse<TResponse> { Url = GetUrl(path) };

        try
        {
            var messageConfiguration = new SoapMessageConfiguration(soapVersion);

            var envelope = GetEnvelope(messageConfiguration, bodies, headers);

            using var requestContent = new StringContent(envelope.ToString(), Encoding.UTF8, messageConfiguration.MediaType);
            response.Request = await requestContent.ReadAsStringAsync();

            if (action != null)
            {
                requestContent.Headers.Add("SOAPAction", action);

                if (messageConfiguration.SoapVersion == SoapVersion.Soap12)
                {
                    requestContent.Headers.ContentType!.Parameters.Add(
                        new NameValueHeaderValue("ActionParameter", $"\"{action}\""));
                }
            }


            var httpResponse = await _client.PostAsync(response.Url, requestContent);
            var content = await httpResponse.Content.ReadAsStringAsync();


            response.Content = content;
            response.StatusCode = httpResponse.StatusCode;

            var deserializedResponse = _soapSerializer.TryDeserializeResponse<TResponse>(soapVersion, content);
            response.Data = deserializedResponse.Data;
            response.ErrorMessage = deserializedResponse.ErrorMessage;

            return response;
        }
        catch (Exception e)
        {
            response.ErrorMessage += e.Message;
        }

        return response;
    }

    private static XElement GetEnvelope(SoapMessageConfiguration soapMessageConfiguration, IEnumerable<XElement> bodies, IEnumerable<XElement>? headers = null)
    {
        var envelope = new XElement(
            soapMessageConfiguration.Schema + "Envelope",
            new XAttribute(XNamespace.Xmlns + "soapenv", soapMessageConfiguration.Schema.NamespaceName));

        envelope.Add(new XElement(soapMessageConfiguration.Schema + "Header", headers));
        envelope.Add(new XElement(soapMessageConfiguration.Schema + "Body", bodies));

        return envelope;
    }

    private Uri GetUrl(string? path) =>
        string.IsNullOrEmpty(path)
            ? _client.TryGetBaseAddress()
            : new Uri(_client.TryGetBaseAddress(), path);
}
