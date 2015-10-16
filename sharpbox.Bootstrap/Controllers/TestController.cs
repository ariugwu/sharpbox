using System.Collections.Generic;
using System.Diagnostics;
using FluentValidation;

namespace sharpbox.Bootstrap.Controllers
{
    using WebLibrary.Web.Controllers;
    using WebLibrary.Core;
    using WebLibrary.Helpers;
    using Models;
    using Common.Dispatch.Model;
    using Dispatch.Model;

    public class TestController : SharpboxController<ExampleModel>
    {
        public CommandName TestCommand = new CommandName("TestCommand");
        public EventName TestEvent = new EventName("TestEvent");
        public CommandName SaveExampleModel = new CommandName("SaveExampleModel");

        public TestController()
            : base(new ExampleAppContext())
        {
            this.BootstrapCrudCommands(this.WebContext.AppContext); // Setup some default crud commands pointed at the default UnitOfWork.

            // Register some example commands.
            this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this.TestCommand, ExampleModel.TestTargetMethod, this.TestEvent);
            this.WebContext.AppContext.Dispatch.Listen(this.TestEvent, (response) => { Debug.WriteLine("We listened and heard: " + ((ExampleModel)response.Entity).Value); });
            this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this.SaveExampleModel, this.UnitOfWork.Add, this.TestEvent);
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

            this.CommandMessageMap.Add(this.SaveExampleModel, new Dictionary<ResponseTypes, string>());
            this.CommandMessageMap[this.SaveExampleModel].Add(ResponseTypes.Success, "Saving the example to the file system was successful!");
        }

        public override ActionCommandMap LoadCommandActionMap()
        {
            return base.LoadCommandActionMap();
        }

        public override Dictionary<CommandName, Dictionary<ResponseTypes, string>> LoadCommandMessageMap(WebContext<ExampleModel> webContext)
        {
            return base.LoadCommandMessageMap(webContext);
        }
    }
}