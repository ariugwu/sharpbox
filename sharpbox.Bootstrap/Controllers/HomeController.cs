using System.Web.Mvc;
using sharpbox.App;
using sharpbox.Bootstrap.Models;
using sharpbox.WebLibrary.Core.Models;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.Bootstrap.Controllers
{
    using App.Model;
    using Dispatch.Model;

    using sharpbox.WebLibrary.Core.Wiring;

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
            this.WebContext.AppContext.Dispatch.Process<AppContext>(BaseWiringCommands.SaveEnvironment, this.CommandMessageMap[BaseWiringCommands.SaveEnvironment][ResponseTypes.Info], new object[] { this.WebContext.AppContext.Environment });

            return this.View("~/Sharpbox/Web/Views/Crud/Index.cshtml");
        }
    }
}
