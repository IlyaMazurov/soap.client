using Soap.Client.Http.Context;

namespace Soap.Client.Http.Logging.Context;

public static class SoapLogAmbient<T> where T : class, ISoapBusinessContext
{
    private static readonly AsyncLocal<SoapLogContext<T>?> Local = new();
    
    public static IDisposable Init(T businessContext)
    {
        Local.Value = new SoapLogContext<T>();
        Local.Value.SetDisposeAction(() => DisposeContext());
        Local.Value.SoapBusinessContext = businessContext;

        return Local.Value;
    }

    internal static void SetSoapClientContext(SoapClientContext context)
    {
        if (Local.Value is not null)
        {
            Local.Value.SoapClientContext = context;
        }
    }

    internal static void SetDisposeAction(Func<Task> disposeAction) 
        => Local.Value?.SetDisposeAction(() => DisposeContext(disposeAction));

    public static SoapLogContext<T>? GetContext() 
        => Local.Value;

    private static void DisposeContext(Func<Task>? disposeAction = null)
    {
        disposeAction?.Invoke().Wait();
        
        Local.Value = null;
    }
}