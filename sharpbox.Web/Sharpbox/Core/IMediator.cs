using sharpbox.Dispatch.Model;

namespace sharpbox.Web.Sharpbox.Core
{
    public interface IMediator<T>
    {
        Feedback<T> Process(AppContext appContext, T instance, CommandNames commandName);

        IDispatchStrategy<T> DispatchStrategy { get; set; }
         
        void RegisterCommands(AppContext appContext);

        void RegisterListeners(AppContext appContext);
    }
}