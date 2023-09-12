using Soap.Client.Http.Context;

namespace Soap.Client.Http.Logging.Context
{
    public class SoapLogContext<T> : IDisposable where T : class, ISoapBusinessContext
    {
        private Action? _disposeAction;

        public SoapClientContext? SoapClientContext { get; set; }
        public T? SoapBusinessContext { get; set; }
        
        internal void SetDisposeAction(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            _disposeAction?.Invoke();
        }
    }
}