using System.Text;
using System.Xml.Linq;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Soap.Client.Model;
using Soap.Client.Http.Context;
using Soap.Client.Serialize;
using Soap.Client.Configuration;
using Soap.Client.Http.Configuration;

namespace Soap.Client.Http;

public class SoapClient : ISoapClient
{
    private const string ClientWorkError = "Error in work Soap client";
    private const string RequestCreated = "Created request: Request={Request}";
    private const string ResponseAccepted = "Accepted response: Response={Response}";
    
    private readonly HttpClient _client;
    private readonly ILogger<SoapClient> _logger;
    private readonly ISoapSerializer _soapSerializer;

    public SoapClient(
        HttpClient client,
        ILogger<SoapClient> logger,
        ISoapSerializer soapSerializer)
    {
        _client = client;
        _logger = logger;
        _soapSerializer = soapSerializer;
    }
    
    public async Task<TResponse> PostAsync<TResponse>(
        SoapVersion soapVersion,
        IEnumerable<XElement> bodies,
        IEnumerable<XElement>? headers = null,
        string? path = null,
        string? action = null) where TResponse : BodyResponse
    {
        var context = new SoapClientContext();
        context.Url = GetUrl(path);

        try
        {
            var messageConfiguration = new SoapMessageConfiguration(soapVersion);

            var envelope = GetEnvelope(messageConfiguration, bodies, headers);

            using var requestContent = new StringContent(envelope.ToString(), Encoding.UTF8, messageConfiguration.MediaType);
            context.Request = await requestContent.ReadAsStringAsync();

            if (action != null)
            {
                requestContent.Headers.Add("SOAPAction", action);

                if (messageConfiguration.SoapVersion == SoapVersion.Soap12)
                {
                    requestContent.Headers.ContentType!.Parameters.Add(
                        new NameValueHeaderValue("ActionParameter", $"\"{action}\""));
                }
            }

            _logger.LogInformation(RequestCreated, envelope.ToString());

            var httpResponse = await _client.PostAsync(context.Url, requestContent);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            _logger.LogInformation(ResponseAccepted, stringResponse);
            context.Response = stringResponse;

            httpResponse.EnsureSuccessStatusCode();

            var response = _soapSerializer.DeserializeResponse<TResponse>(soapVersion, stringResponse);

            return response;
        }
        catch (Exception e)
        {
            _logger.LogError(e, ClientWorkError);
            context.ErrorMessage = e.Message;

            throw;
        }
        finally
        {
            SoapClientAmbient.SetContext(context);
        }
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

    private string GetUrl(string? path) => _client.BaseAddress + path;
}