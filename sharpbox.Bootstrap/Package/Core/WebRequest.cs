using sharpbox.Dispatch.Model;

namespace sharpbox.WebLibrary.Core
{
    public class WebRequest<T>
    {
        public CommandName CommandName { get; set; }
        public T Instance { get; set; }

    }
}