using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;

namespace sharpbox.Bootstrap.Package.Core
{
  public class WebResponse<T>
  {
    public T Instance { get; set; }

    public Stack<ModelError> ModelErrors { get; set; }

    public Response Response { get; set; }

    public WebRequest<T> WebRequest { get; set; } 
  }
}