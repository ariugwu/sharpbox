using System.Collections.Generic;
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
            this.BootstrapCrudCommands(this.WebContext.AppContext); // Setup some default crud commands pointed at the default UnitOfWork.
            
            // Register some example commands.
            this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this._testCommand, ExampleModel.TestTargetMethod, this._testEvent);
            this.WebContext.AppContext.Dispatch.Listen(_testEvent, (response) => { Debug.WriteLine("We listened and heard: " + ((ExampleModel)response.Entity).Value); });
            this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this._saveExampleModel, this.UnitOfWork.Insert, _testEvent);
        }

        public override AbstractValidator<ExampleModel> LoadValidatorByUiAction(UiAction uiAction)
        {
            var validator = new InlineValidator<ExampleModel>();
            validator.RuleFor(x => x.Value).Length(10, 30);
            return validator;
        }

      public override void BootstrapCrudCommands(AppContext appContext)
      {
        base.BootstrapCrudCommands(appContext);

        this.CommandMessageMap.Add(_saveExampleModel, new Dictionary<ResponseTypes, string>());
        this.CommandMessageMap[_saveExampleModel].Add(ResponseTypes.Success, "Saving the example to the file system was successful!");
      }

    }
}