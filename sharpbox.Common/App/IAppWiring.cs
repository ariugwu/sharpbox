namespace sharpbox.Common.App
{
    using sharpbox.Common.Dispatch;

    public interface IAppWiring
    {
        void WireDefaultRoutes<T>(IDispatchContext dispatchContext) where T : class, new();

        void WireContext<T>(IDispatchContext dispatchContext) where T : class, new();

    }
}