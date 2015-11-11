using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;

using FluentValidation;

namespace sharpbox.Bootstrap.Controllers
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    using Common.Data.Helpers;
    using Common.Dispatch.Model;

    using Dispatch.Model;

    using WebLibrary.Core;
    using WebLibrary.Core.Models;
    using WebLibrary.Helpers;
    using WebLibrary.Helpers.ControllerWiring;
    using WebLibrary.Web.Controllers;

    public sealed class TestController : SharpboxScaffoldController<ExampleModel>
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

        public override Dictionary<object, Attribute> GetDataAnnotations()
        {
            var entity = new ExampleModel {Value = "", FirstName = ""}; // Seems like in order to reference the object in the dictionary it can't be null.

            var annotationMap = new Dictionary<object, Attribute>();
                annotationMap.Add(entity.Age, new MaxLengthAttribute(15));
                annotationMap.Add(entity.FirstName, new RequiredAttribute());

            return annotationMap;
        }

        public ActionResult Seed()
        {
            var list = new List<ExampleModel>();
            list.Add(new ExampleModel() { SharpId = Guid.NewGuid(), Age = 1, BirthDate = DateTime.Now.AddDays(1), FirstName = "Sally", LastName = "Ranch", Value = "A" });
            list.Add(new ExampleModel() { SharpId = Guid.NewGuid(), Age = 2, BirthDate = DateTime.Now.AddDays(2), FirstName = "Mark", LastName = "Resiling", Value = "B" });
            list.Add(new ExampleModel() { SharpId = Guid.NewGuid(), Age = 3, BirthDate = DateTime.Now.AddDays(3), FirstName = "Jason", LastName = "Brooks", Value = "C" });
            list.Add(new ExampleModel() { SharpId = Guid.NewGuid(), Age = 4, BirthDate = DateTime.Now.AddDays(4), FirstName = "Alex", LastName = "Tinsley", Value = "D" });
            list.Add(new ExampleModel() { SharpId = Guid.NewGuid(), Age = 5, BirthDate = DateTime.Now.AddDays(5), FirstName = "Brian", LastName = "Walker", Value = "E" });
            list.Add(new ExampleModel() { SharpId = Guid.NewGuid(), Age = 6, BirthDate = DateTime.Now.AddDays(6), FirstName = "Steven", LastName = "Stokes", Value = "F" });
            list.Add(new ExampleModel() { SharpId = Guid.NewGuid(), Age = 7, BirthDate = DateTime.Now.AddDays(7), FirstName = "Mike", LastName = "Jackson", Value = "G" });
            list.Add(new ExampleModel() { SharpId = Guid.NewGuid(), Age = 8, BirthDate = DateTime.Now.AddDays(8), FirstName = "Nick", LastName = "Lancaster", Value = "H" });
            list.Add(new ExampleModel() { SharpId = Guid.NewGuid(), Age = 9, BirthDate = DateTime.Now.AddDays(9), FirstName = "Josh", LastName = "Holmes", Value = "I" });

            this.WebContext.AppContext.Dispatch.Process<List<ExampleModel>>(
                DefaultAppWiring.UpdateAll,
                "Seeding the collection",
                new object[] { list });

            return this.RedirectToAction("Index");
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