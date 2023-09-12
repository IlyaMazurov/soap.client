using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Soap.Client.Http.Context;
using Soap.Client.Http.Logging.Logger;
using Soap.Client.Http.Logging.Context;

namespace Soap.Client.Test.Logger;

public class CustomSoapClientLogger : ISoapClientLogger<CustomSoapBusinessContext>
{
    private readonly ILogger<CustomSoapClientLogger> _logger;

    public CustomSoapClientLogger(ILogger<CustomSoapClientLogger> logger)
    {
        _logger = logger;
    }

    public Task LogAsync(SoapLogContext<CustomSoapBusinessContext>? context)
    {
        _logger.LogInformation("Данные бизнесконтекста: {Name}, {Result}", context?.SoapBusinessContext?.Name, context?.SoapBusinessContext?.Result);
        
        _logger.LogInformation("Кастомное логировавние:\n Бизнес контекст: {businessContext}\n Контекст клиента: {clientLogContext}", 
            JsonConvert.SerializeObject(context?.SoapBusinessContext),
            JsonConvert.SerializeObject(TryGetSoapClientContext(context)));
        
        return Task.CompletedTask;
    }

    private static SoapClientContext TryGetSoapClientContext(SoapLogContext<CustomSoapBusinessContext>? context)
    {
        if (context?.SoapClientContext is null)
        {
            throw new InvalidOperationException(nameof(context.SoapClientContext));
        }

        return context.SoapClientContext;
    }
}