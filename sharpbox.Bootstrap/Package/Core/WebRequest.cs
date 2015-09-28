﻿using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Helpers;

namespace sharpbox.WebLibrary.Core
{
    using System;

    [Serializable]
    public class WebRequest<T>
    {
        public UiAction UiAction { get; set; }
        public CommandName CommandName { get; set; }
        public T Instance { get; set; }

    }
}