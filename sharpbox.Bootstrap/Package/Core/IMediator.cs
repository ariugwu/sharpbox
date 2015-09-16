using sharpbox.Dispatch.Model;

namespace sharpbox.WebLibrary.Core
{
    public interface IMediator<T>
    {
        IDispatchStrategy<T> DispatchStrategy { get; set; }

        void RegisterCommands(WebContext<T> webContext);

        void RegisterListeners(WebContext<T> webContext);
    }
}