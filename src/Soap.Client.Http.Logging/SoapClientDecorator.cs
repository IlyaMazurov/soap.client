using Soap.Client.Configuration;
using Soap.Client.Model;
using System.Xml.Linq;
using Soap.Client.Http.Context;
using Soap.Client.Http.Logging.Context;
using Soap.Client.Http.Logging.Logger;

namespace Soap.Client.Http.Logging
{
    public class SoapClientDecorator<T> : ISoapClient where T : class, ISoapBusinessContext
    {
        private readonly ISoapClient _client;
        private readonly ISoapClientLogger<T> _logger;

        public SoapClientDecorator(
            ISoapClient client,
            ISoapClientLogger<T> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<TResponse> PostAsync<TResponse>(
            SoapVersion soapVersion,
            IEnumerable<XElement> bodies,
            IEnumerable<XElement>? headers = null,
            string? path = null,
            string? action = null) where TResponse : BodyResponse
        {
            using (SoapClientAmbient.Init())
            {
                SoapLogAmbient<T>.SetDisposeAction(Log);

                try
                {
                    return await _client.PostAsync<TResponse>(soapVersion, bodies, headers, path, action);
                }
                finally
                {
                    if (SoapLogAmbient<T>.GetContext() is not null)
                    {
                        SoapLogAmbient<T>.SetSoapClientContext(SoapClientAmbient.GetContext());
                    }
                }
                
                async Task Log()
                {
                    if (SoapLogAmbient<T>.GetContext() is not null)
                    {
                        await _logger.LogAsync(SoapLogAmbient<T>.GetContext());
                    }
                }
            }
        }
    }
}