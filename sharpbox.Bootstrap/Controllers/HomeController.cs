using System.Linq;
using System.Web.Mvc;
using sharpbox.WebLibrary.Helpers.TypeScript;

namespace sharpbox.Bootstrap.Controllers
{
    using FluentValidation;

    using sharpbox.Bootstrap.Models;
    using sharpbox.WebLibrary.Helpers;
    using sharpbox.WebLibrary.Web.Controllers;

    public class HomeController : SharpboxController<ExampleModel>
    {
        public HomeController() : base(new ExampleAppContext())
        {
            this.BootstrapCrudCommands(this.WebContext.AppContext);
        }

        // GET: Home
        public ActionResult Index()
        {
            var test = new DomainMetadata();

            var names = MetaLoader.GetAllMetaDataClasses(); //test.Wat();

            var fields = names.SelectMany(x => x.GetFields()).ToList();

            var hmm = fields.SelectMany(x => x.Name);

            var woo = names.SelectMany(x => x.Name);

            var blerg = fields.ToArray();

            var arg = hmm.ToList();

            var clerg = woo.ToList();

            return View();
        }

        public override AbstractValidator<ExampleModel> LoadValidatorByUiAction(UiAction uiAction)
        {
            throw new System.NotImplementedException();
        }
    }
}
