using System;
using System.Net.Mail;
using System.Web.Mvc;
using FluentValidation;
using sharpbox.Io.Strategy.Binary;
using sharpbox.WebLibrary.Web.Helpers;

namespace sharpbox.Bootstrap.Controllers
{
    using sharpbox.Dispatch.Model;

    public class HomeController : sharpbox.WebLibrary.Web.Controllers.SharpboxController<string>
    {
        private CommandName _testCommand = new CommandName("TestCommand");
        private EventName _testEvent = new EventName("TestEvent");

        public HomeController()
            : base(new AppContext(new SmtpClient(), new BinaryStrategy()))
        {
            this.WebContext.AppContext.Dispatch.Register<string>(this._testCommand, (value) => value + "...I changed this",this._testEvent);
        }

        public ActionResult Index()
        {
            return View();
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

        public override AbstractValidator<string> LoadValidatorByUiAction(UiAction uiAction)
        {
            return new InlineValidator<string>();
        }

        public override ActionCommandMap LoadCommandActionMap()
        {
            return new ActionCommandMap(useOneToOneMap: true);
        }
    }
}