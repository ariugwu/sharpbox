using System.Collections.Generic;
using System.Diagnostics;
using FluentValidation;

namespace sharpbox.Bootstrap.Controllers
{
    using Common.Dispatch.Model;
    using Dispatch.Model;
    using Models;
    using WebLibrary.Core;
    using WebLibrary.Helpers;
    using WebLibrary.Web.Controllers;

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

        /// <summary>
        /// Provides the option to keep all of the default application wiring but provide your own routes for Get, Add, Update, Remove
        /// For example you might want to provide your own persistence strategy on top of the base wiring but also point to Default routes outside of that strategy.
        /// </summary>
        public override void WireDefaultRoutes()
        {
            base.WireDefaultRoutes(); // Handles wiring of Get, Add, Update, Remove. Base functionality is to use the file persistence
        }

        /// <summary>
        /// Only override if you want to provide your own application wiring. Very much a "everyone thinks they want to, but no one should" scenario.
        /// </summary>
        public override void WireApplicationContext()
        {
            base.WireApplicationContext(); // Handles the wiring for all of the application persistence. Environment, Membership, Localization, etc. Defaults to file persistence.
        }

        /// <summary>
        /// Place additional domain specific wiring here.
        /// </summary>
        public override void WireDomain()
        {
            this.CommandMessageMap.Add(this.SaveExampleModel, new Dictionary<ResponseTypes, string>());
            this.CommandMessageMap[this.SaveExampleModel].Add(ResponseTypes.Success, "Saving the example to the file system was successful!");

            // Register some example commands.
            this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this.TestCommand, ExampleModel.TestTargetMethod, this.TestEvent);
            this.WebContext.AppContext.Dispatch.Listen(this.TestEvent, (response) => { Debug.WriteLine("We listened and heard: " + ((ExampleModel)response.Entity).Value); });
            //this.WebContext.AppContext.Dispatch.Register<ExampleModel>(this.SaveExampleModel, this.UnitOfWork.Add, this.TestEvent);
        }

        /// <summary>
        /// You can map UiActions to Commands explicitly. The default behavior is to assume if a UiAction comes in there will be matching CommandName registered with the dispatcher
        /// </summary>
        /// <returns></returns>
        public override ActionCommandMap LoadCommandActionMap()
        {
            return base.LoadCommandActionMap();
        }

        /// <summary>
        /// Provide custom messages for your commands. One for each response type (Success, Error, Info)
        /// </summary>
        /// <param name="webContext"></param>
        /// <returns></returns>
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