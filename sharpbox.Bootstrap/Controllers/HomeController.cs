using System.Web.Mvc;
using sharpbox.App;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.Bootstrap.Controllers
{
    using App.Model;
    using Common.App;
    using Common.Dispatch;

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

            this.WebContext.Environment.BaseUrl = "http://sharpbox.io";
            this.WebContext.Dispatch.Process<AppContext>(BaseCommandName.SaveEnvironment, this.CommandMessageMap[BaseCommandName.SaveEnvironment][ResponseTypes.Info], new object[] { this.WebContext.Environment });

            return this.View("~/Sharpbox/Web/Views/Crud/Index.cshtml");
        }
    }
}
