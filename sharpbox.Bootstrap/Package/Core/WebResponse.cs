using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace sharpbox.WebLibrary.Core
{
  [Serializable]
  public class WebResponse<T>
  {
    public T Instance { get; set; }

    public Dictionary<string,Stack<ModelError>> ModelErrors { get; set; }

    public string ResponseType { get; set; }

    public string Message { get; set; }

  }
}