using System.Collections.Generic;
using System.Web.Mvc;
using sharpbox.Dispatch.Model;

namespace sharpbox.WebLibrary.Core
{
  public class WebResponse<T>
  {
    public T Instance { get; set; }

    public Dictionary<string,Stack<ModelError>> ModelErrors { get; set; }

    public Response DispatchResponse { get; set; }

    public WebRequest<T> WebRequest { get; set; }

    public string CustomMessage { get; set; }

    public bool HasCustomMessage { get { return !string.IsNullOrWhiteSpace(this.CustomMessage); } }

    public bool IsValid
    {
      get { return this.ModelErrors == null || this.ModelErrors.Count == 0; }
    }
  }
}