namespace Soap.Client.Http.Context;

public static class SoapClientAmbient
{
    private static readonly AsyncLocal<SoapClientContext?> Local = new();

    public static IDisposable Init()
    {
        Local.Value = new SoapClientContext();
        Local.Value.SetDisposeAction(DisposeContext);

        return Local.Value;
    }

    internal static void SetContext(SoapClientContext context)
    {
        if (Local.Value == null) return;

        Local.Value.Url = context.Url;
        Local.Value.Request = context.Request;
        Local.Value.Response = context.Response;
        Local.Value.ErrorMessage = context.ErrorMessage;
    }

    public static SoapClientContext GetContext()
        => Local.Value
           ?? throw new InvalidOperationException("Failed to get soap-client integration context. The context is not initialized.");

    private static void DisposeContext()
        => Local.Value = null;
}