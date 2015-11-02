﻿namespace sharpbox.Common.Data.Core
{
    using System;
    using Common.Dispatch.Model;
    using Helpers;

    [Serializable]
    public class WebRequest<T>
    {
        public UiAction UiAction { get; set; }
        public CommandName CommandName { get; set; }
        public T Instance { get; set; }

    }
}