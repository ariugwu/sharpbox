using System;
using System.Linq;
using System.Web.Mvc;
using sharpbox.WebLibrary.Core;
    
namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
    public class ValidationHandler<T> : LifecycleHandler<T>
    {
        public override void HandleRequest(WebContext<T> webContext, Controller controller)
        {
            if (!controller.ModelState.IsValid)
            {
                webContext.ModelStateErrors = controller.ModelState.Values.SelectMany(x => x.Errors);
            }
            else if (!webContext.Validate())
            {
                foreach (var e in webContext.ValidationResult.Errors)
                {
                    controller.ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                }
            }
        }
    }
}