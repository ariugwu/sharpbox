
using System.Diagnostics;
using System.Net.Mail;
using System.Web.Mvc;
using FluentValidation;
using sharpbox.Bootstrap.Models;
using sharpbox.Dispatch.Model;
using sharpbox.Io.Strategy.Binary;
using sharpbox.WebLibrary.Web.Helpers;

namespace sharpbox.Bootstrap.Controllers
{
    using sharpbox.Bootstrap.Package.Core;

    public class HomeController : sharpbox.WebLibrary.Web.Controllers.SharpboxController<ExampleModel>
    {
        private CommandName _testCommand = new CommandName("TestCommand");
        private EventName _testEvent = new EventName("TestEvent");

        public HomeController()
          : base(new AppContext(new SmtpClient(), new BinaryStrategy()))
        {
            this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this._testCommand, ExampleModel.TestTargetMethod, this._testEvent);
            this.WebContext.AppContext.Dispatch.Listen(_testEvent, (response) => { Debug.WriteLine("We listened and heard: " + ((ExampleModel)response.Entity).Value); });
        }

        public ActionResult Index()
        {
            return this.View(this.WebContext.WebResponse ?? new WebResponse<ExampleModel>());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public override AbstractValidator<ExampleModel> LoadValidatorByUiAction(UiAction uiAction)
        {
            var validator = new InlineValidator<ExampleModel>();
            validator.RuleFor(x => x.Value).Length(10, 30);
            return validator;
        }

        public override ActionCommandMap LoadCommandActionMap()
        {
            return new ActionCommandMap(useOneToOneMap: true);
        }

    }
}