using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sharpbox.Bootstrap.Controllers
{
    using Models;

    using sharpbox.WebLibrary.Core;
    using sharpbox.WebLibrary.Helpers.ControllerWiring;

    using WebLibrary.Web.Controllers;

    public class TestListController : SharpboxController<List<ExampleModel>> {

        public TestListController()
            : base(new ExampleAppContext())
        {
        }

        public ActionResult Seed()
        {
            var list = new List<ExampleModel>();
            list.Add(new ExampleModel() { Age = 1, BirthDate = DateTime.Now.AddDays(1), FirstName = "Sally", LastName = "Ranch", Value = "A"});
            list.Add(new ExampleModel() { Age = 2, BirthDate = DateTime.Now.AddDays(2), FirstName = "Mark", LastName = "Resiling", Value = "B" });
            list.Add(new ExampleModel() { Age = 3, BirthDate = DateTime.Now.AddDays(3), FirstName = "Jason", LastName = "Brooks", Value = "C" });
            list.Add(new ExampleModel() { Age = 4, BirthDate = DateTime.Now.AddDays(4), FirstName = "Alex", LastName = "Tinsley", Value = "D" });
            list.Add(new ExampleModel() { Age = 5, BirthDate = DateTime.Now.AddDays(5), FirstName = "Brian", LastName = "Walker", Value = "E" });
            list.Add(new ExampleModel() { Age = 6, BirthDate = DateTime.Now.AddDays(6), FirstName = "Steven", LastName = "Stokes", Value = "F" });
            list.Add(new ExampleModel() { Age = 7, BirthDate = DateTime.Now.AddDays(7), FirstName = "Mike", LastName = "Jackson", Value = "G" });
            list.Add(new ExampleModel() { Age = 8, BirthDate = DateTime.Now.AddDays(8), FirstName = "Nick", LastName = "Lancaster", Value = "H" });
            list.Add(new ExampleModel() { Age = 9, BirthDate = DateTime.Now.AddDays(9), FirstName = "Josh", LastName = "Holmes", Value = "I" });

            this.WebContext.AppContext.Dispatch.Process<List<ExampleModel>>(
                DefaultAppWiring.Update,
                "Seeding the collection",
                new object[] { list });

            return this.RedirectToAction("Index");
        }
    }
}