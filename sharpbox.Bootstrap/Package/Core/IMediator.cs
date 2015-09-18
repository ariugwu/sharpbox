using sharpbox.Dispatch.Model;

namespace sharpbox.WebLibrary.Core
{
    public interface IMediator<T>
    {
        IDispatchStrategy<T> DispatchStrategy { get; set; }
    }
}