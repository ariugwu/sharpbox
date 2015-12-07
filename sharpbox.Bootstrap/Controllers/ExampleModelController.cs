using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;

using FluentValidation;
using sharpbox.Bootstrap.Models;

namespace sharpbox.Bootstrap.Controllers
{
    using Common.App;
    using Common.Data.Helpers;
    using Common.Dispatch;
    using Common.Dispatch.Model;

    using WebLibrary.Helpers;
    using WebLibrary.Web.Controllers;

    public sealed class ExampleModelController : SharpboxController<ExampleModel>
    {
        public ActionResult Seed()
        {
            var list = new List<ExampleModel>();
            list.Add(new ExampleModel() { ExampleModelId = 9, Age = 1, BirthDate = DateTime.Now.AddDays(1), FirstName = "Sally", LastName = "Ranch", Value = "A", ExampleChildId = 1 });
            list.Add(new ExampleModel() { ExampleModelId = 8, Age = 2, BirthDate = DateTime.Now.AddDays(2), FirstName = "Mark", LastName = "Resiling", Value = "B", ExampleChildId = 3 });
            list.Add(new ExampleModel() { ExampleModelId = 7, Age = 3, BirthDate = DateTime.Now.AddDays(3), FirstName = "Jason", LastName = "Brooks", Value = "C", ExampleChildId = 2 });
            list.Add(new ExampleModel() { ExampleModelId = 6, Age = 4, BirthDate = DateTime.Now.AddDays(4), FirstName = "Alex", LastName = "Tinsley", Value = "D", ExampleChildId = 1 });
            list.Add(new ExampleModel() { ExampleModelId = 5, Age = 5, BirthDate = DateTime.Now.AddDays(5), FirstName = "Brian", LastName = "Walker", Value = "E", ExampleChildId = 4 });
            list.Add(new ExampleModel() { ExampleModelId = 4, Age = 6, BirthDate = DateTime.Now.AddDays(6), FirstName = "Steven", LastName = "Stokes", Value = "F", ExampleChildId = 1 });
            list.Add(new ExampleModel() { ExampleModelId = 3, Age = 7, BirthDate = DateTime.Now.AddDays(7), FirstName = "Mike", LastName = "Jackson", Value = "G", ExampleChildId = 1 });
            list.Add(new ExampleModel() { ExampleModelId = 2, Age = 8, BirthDate = DateTime.Now.AddDays(8), FirstName = "Nick", LastName = "Lancaster", Value = "H", ExampleChildId = 1 });
            list.Add(new ExampleModel() { ExampleModelId = 1, Age = 9, BirthDate = DateTime.Now.AddDays(9), FirstName = "Josh", LastName = "Holmes", Value = "I", ExampleChildId = 1 });

            this.WebContext.Dispatch.Process<List<ExampleModel>>(
                BaseCommandName.UpdateAll,
                "Seeding the collection",
                new object[] { list });

            return this.RedirectToAction("Index");
        }

        public override AbstractValidator<ExampleModel> LoadValidatorByUiAction(UiAction uiAction)
        {
            var validator = new InlineValidator<ExampleModel>();
            validator.RuleFor(x => x.Value).Length(10, 30);
            return validator;
        }

        /// <summary>
        /// Provides the option to keep all of the default application wiring but provide your own routes for Get, GetById, Add, Update, Remove
        /// For example you might want to provide your own persistence strategy on top of the base wiring but also point to Default routes outside of that strategy.
        /// </summary>
        public override void WireDefaultRoutes()
        {
            base.WireDefaultRoutes(); // Handles wiring of Get, Add, Update, Remove. Base functionality is to use the file persistence
        }

        /// <summary>
        /// Only override if you want to provide your own application wiring. Very much a "everyone thinks they want to, but no one should" scenario.
        /// * Ideally will be set once per shared environment for things like Database, Email, Localization, Membership, Io
        /// </summary>
        public override void WireApplicationContext()
        {
            base.WireApplicationContext(); // Handles the wiring for all of the application persistence. Environment, Membership, Localization, etc. Defaults to file persistence.
        }

        /// <summary>
        /// Place additional domain specific wiring here. i.e. - You can use this to simply append command messages or (dispatch) registrations instead of overriding base functionality.
        /// </summary>
        public override void WireDomain()
        {
            this.CommandMessageMap.Add(this.SaveExampleModel, new Dictionary<ResponseTypes, string>());
            this.CommandMessageMap[this.SaveExampleModel].Add(ResponseTypes.Success, "Saving the example to the file system was successful!");

            // Register some example commands.
            this.WebContext.Dispatch.Register<ExampleModel>(this.TestCommand, ExampleModel.TestTargetMethod, this.TestEvent);
            this.WebContext.Dispatch.Listen(this.TestEvent, (response) => { Debug.WriteLine("We listened and heard: " + ((ExampleModel)response.Entity).Value); });
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
        /// <returns></returns>
        public override Dictionary<CommandName, Dictionary<ResponseTypes, string>> LoadCommandMessageMap()
        {
            return base.LoadCommandMessageMap();
        }

        public CommandName TestCommand = new CommandName("TestCommand");
        public EventName TestEvent = new EventName("TestEvent");
        public CommandName SaveExampleModel = new CommandName("SaveExampleModel");
        public UiAction CouldBeAnything = new UiAction("CouldBeAnything");

    }
}