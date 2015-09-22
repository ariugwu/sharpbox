﻿using System.Collections.Generic;
using System.Web.Mvc;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;

namespace sharpbox.Bootstrap.Package.Core
{
  public class WebResponse<T>
  {
    public T Instance { get; set; }

    public Dictionary<string,Stack<ModelError>> ModelErrors { get; set; }

    public Response DispatchResponse { get; set; }

    public WebRequest<T> WebRequest { get; set; }

    public bool IsValid
    {
      get { return ModelErrors == null || ModelErrors.Count == 0; }
    }
  }
}