using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Soap.Client.Configuration;
using Soap.Client.Extensions;
using Soap.Client.Models;

namespace Soap.Client.Decorate;

public class SoapClientLoggingDecorator : ISoapClient
{
    private const string ClientWorkError = "Error in work Soap client";
    private const string RequestCreated = "Created request: Request={Request}";
    private const string ResponseAccepted = "Accepted response: Response={Response}";

    private readonly ISoapClient _client;
    private readonly ILogger<SoapClientLoggingDecorator> _logger;

    public SoapClientLoggingDecorator(
        ISoapClient client,
        ILogger<SoapClientLoggingDecorator> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<SoapResponse<TResponse>> PostAsync<TResponse>(
        SoapVersion soapVersion,
        IEnumerable<XElement> bodies,
        IEnumerable<XElement>? headers = null,
        string? path = null,
        string? action = null) where TResponse : BodyResponse
    {
        var response = await _client.PostAsync<TResponse>(soapVersion, bodies, headers, path, action);

        _logger.LogRequest(RequestCreated, response.Request);

        if (!string.IsNullOrWhiteSpace(response.Content))
        {
            _logger.LogResponse(ResponseAccepted, response.Content);
        }

        if (!response.Success)
        {
            _logger.LogErrorMessage(ClientWorkError,
                response.ErrorMessage ?? throw new InvalidOperationException("ErrorMessage is empty"));
        }

        return response;
    }
}
