namespace Soap.Client.Http.Context;

public sealed class SoapClientContext : IDisposable
{
    private Action? _disposeAction;

    public string? Request { get; set; }
    public string? Response { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Url { get; set; }

    internal void SetDisposeAction(Action disposeAction)
    {
        _disposeAction = disposeAction;
    }

    public void Dispose()
    {
        _disposeAction?.Invoke();
    }
}