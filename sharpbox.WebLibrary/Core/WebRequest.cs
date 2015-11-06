using sharpbox.WebLibrary.Helpers;

namespace sharpbox.WebLibrary.Core
{
    using System;
    using Common.Dispatch.Model;

    [Serializable]
    public class WebRequest<T>
    {
        public UiAction UiAction { get; set; }
        public CommandName CommandName { get; set; }
        public T Instance { get; set; }
        public Io.Model.FileDetail FileDetail { get; set; }

    }
}