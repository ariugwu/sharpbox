using System.Web.Mvc;
using sharpbox.App;
using sharpbox.WebLibrary.Core.Models;
using sharpbox.WebLibrary.Helpers.ControllerWiring;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.Bootstrap.Controllers
{
    using App.Model;
    using Dispatch.Model;

    public sealed class HomeController : SharpboxScaffoldController<Environment>
    {
        public HomeController() : base(new ExampleAppContext())
        {
            
        }

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

            this.WebContext.AppContext.Environment.BaseUrl = "http://sharpbox.io";
            this.WebContext.AppContext.Dispatch.Process<AppContext>(DefaultAppWiring.SaveEnvironment, this.CommandMessageMap[DefaultAppWiring.SaveEnvironment][ResponseTypes.Info], new object[] { this.WebContext.AppContext.Environment });

            return this.View("~/Sharpbox/Web/Views/Crud/Index.cshtml");
        }
    }
}
