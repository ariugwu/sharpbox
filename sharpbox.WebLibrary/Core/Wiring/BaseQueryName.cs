﻿namespace sharpbox.WebLibrary.Core.Wiring
{
    using sharpbox.Common.Dispatch.Model;

    public class BaseQueryName
    {
        public static QueryName Get = new QueryName("Get");
        public static QueryName GetById = new QueryName("GetId");
    }
}
