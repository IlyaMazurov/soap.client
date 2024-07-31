using Soap.Client.Configuration;
using Soap.Client.Models;

namespace Soap.Client.Extensions;

public static class SoapClientExtensions
{
    public static async Task<SoapResponse<TResponse>> PostAsync<TResponse>(
        this ISoapClient soapClient,
        SoapVersion soapVersion,
        object body,
        object header,
        string? path = null,
        string? action = null) where TResponse : BodyResponse
    {
        var xBody = body.CreateXElement();
        var xHeader = header.CreateXElement();

        return await soapClient.PostAsync<TResponse>(soapVersion, new[] { xBody }, new[] { xHeader }, path, action);
    }

    public static async Task<SoapResponse<TResponse>> PostAsync<TResponse>(
        this ISoapClient soapClient,
        SoapVersion soapVersion,
        object body,
        string? path = null,
        string? action = null) where TResponse : BodyResponse
    {
        var xBody = body.CreateXElement();
        return await soapClient.PostAsync<TResponse>(soapVersion, new[] { xBody }, null, path, action);
    }
}
