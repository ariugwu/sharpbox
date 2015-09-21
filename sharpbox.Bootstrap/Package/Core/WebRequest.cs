using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Web.Helpers;

namespace sharpbox.WebLibrary.Core
{
    public class WebRequest<T>
    {
        public UiAction UiAction { get; set; }
        public CommandName CommandName { get; set; }
        public T Instance { get; set; }

    }
}