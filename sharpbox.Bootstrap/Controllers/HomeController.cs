using System.Web.Mvc;
using sharpbox.App;
using sharpbox.WebLibrary.Helpers.TypeScript;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.Bootstrap.Controllers
{
    using App.Model;

    using sharpbox.Dispatch.Model;

    public sealed class HomeController : SharpboxController<Environment>
    {
        // GET: Home
        public ActionResult Test()
        {
            //var test = new DomainMetadata();

            //var names = MetaLoader.GetAllMetaDataClasses(); //test.Wat();

            //var fields = names.SelectMany(x => x.GetFields()).ToList();

            //var hmm = fields.SelectMany(x => x.Name);

            //var woo = names.SelectMany(x => x.Name);

            //var blerg = fields.ToArray();

            //var arg = hmm.ToList();

            //var clerg = woo.ToList();

            return this.View("~/Sharpbox/Web/Views/Crud/Index.cshtml");
        }
    }
}
