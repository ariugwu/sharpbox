using System.Diagnostics;
using System.Web.Mvc;
using FluentValidation;
using sharpbox.Bootstrap.Models;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Helpers;

namespace sharpbox.Bootstrap.Controllers
{
    using sharpbox.WebLibrary.Mvc.Controllers;

    public class TestController : SharpboxApiController<ExampleModel>
    {
        private CommandName _testCommand = new CommandName("TestCommand");
        private EventName _testEvent = new EventName("TestEvent");
        private CommandName _saveExampleModel = new CommandName("SaveExampleModel");

        public TestController()
          : base(new ExampleAppContext())
        {
            this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this._testCommand, ExampleModel.TestTargetMethod, this._testEvent);
            this.WebContext.AppContext.Dispatch.Listen(_testEvent, (response) => { Debug.WriteLine("We listened and heard: " + ((ExampleModel)response.Entity).Value); });
            this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this._saveExampleModel, this.WebContext.Mediator.UnitOfWork.Insert, _testEvent);
        }

        public override AbstractValidator<ExampleModel> LoadValidatorByUiAction(UiAction uiAction)
        {
            var validator = new InlineValidator<ExampleModel>();
            validator.RuleFor(x => x.Value).Length(10, 30);
            return validator;
        }

    }
}