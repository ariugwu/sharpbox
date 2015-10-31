using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Mvc;
using FluentValidation;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using sharpbox.WebLibrary.Helpers.ControllerWiring;

namespace sharpbox.Bootstrap.Controllers
{
    using WebLibrary.Web.Controllers;
    using WebLibrary.Core;
    using WebLibrary.Helpers;
    using Models;
    using Common.Dispatch.Model;
    using Dispatch.Model;

    public sealed class TestController : SharpboxController<ExampleModel>
    {
        public TestController()
            : base(new ExampleAppContext())
        {
        }

        public override AbstractValidator<ExampleModel> LoadValidatorByUiAction(UiAction uiAction)
        {
            var validator = new InlineValidator<ExampleModel>();
            validator.RuleFor(x => x.Value).Length(10, 30);
            return validator;
        }

        public override void WireApplication()
        {
            this.CommandMessageMap.Add(this.SaveExampleModel, new Dictionary<ResponseTypes, string>());
            this.CommandMessageMap[this.SaveExampleModel].Add(ResponseTypes.Success, "Saving the example to the file system was successful!");

            // Register some example commands.
            this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this.TestCommand, ExampleModel.TestTargetMethod, this.TestEvent);
            this.WebContext.AppContext.Dispatch.Listen(this.TestEvent, (response) => { Debug.WriteLine("We listened and heard: " + ((ExampleModel)response.Entity).Value); });
            //this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this.SaveExampleModel, this.UnitOfWork.Add, this.TestEvent);
        }

        public override ActionCommandMap LoadCommandActionMap()
        {
            return base.LoadCommandActionMap();
        }

        public override Dictionary<CommandName, Dictionary<ResponseTypes, string>> LoadCommandMessageMap(WebContext<ExampleModel> webContext)
        {
            return base.LoadCommandMessageMap(webContext);
        }

        public CommandName TestCommand = new CommandName("TestCommand");
        public EventName TestEvent = new EventName("TestEvent");
        public CommandName SaveExampleModel = new CommandName("SaveExampleModel");
        public UiAction CouldBeAnything = new UiAction("CouldBeAnything");

    }
}