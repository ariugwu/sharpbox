using System;
using System.Collections.Generic;
using System.Web.Mvc;
using sharpbox.Bootstrap.Models;

namespace sharpbox.Bootstrap.Controllers
{
    using Common.App;

    using sharpbox.App;

    using WebLibrary.Web.Controllers;

    public class ExampleChildController : SharpboxController<ExampleChild>
    {
        public ActionResult Seed()
        {
            var list = new List<ExampleChild>();
            list.Add(new ExampleChild() { ExampleChildId = 1, ARandomNumber = 543510, CreatedDate = DateTime.Now.AddDays(1), IsValid = true, SomeDoubleNumber = 9323.2, Title = "A random title." });
            list.Add(new ExampleChild() { ExampleChildId = 2, ARandomNumber = 10435, CreatedDate = DateTime.Now.AddDays(31), IsValid = true, SomeDoubleNumber = 67923.2, Title = "Should really use a fakes library" });
            list.Add(new ExampleChild() { ExampleChildId = 3, ARandomNumber = 12420, CreatedDate = DateTime.Now.AddDays(51), IsValid = true, SomeDoubleNumber = 499283.2, Title = "Blerg. Please use Fakes!" });
            list.Add(new ExampleChild() { ExampleChildId = 9, ARandomNumber = 6610, CreatedDate = DateTime.Now.AddDays(12), IsValid = true, SomeDoubleNumber = 198823.2, Title = "Making up titles is hard." });
            list.Add(new ExampleChild() { ExampleChildId = 4, ARandomNumber = 710, CreatedDate = DateTime.Now.AddDays(-13), IsValid = true, SomeDoubleNumber = 335923.2, Title = "For science!" });
            this.WebContext.Dispatch.Process<List<ExampleChild>>(
                AppContext.UpdateAll,
                "Seeding the collection",
                new object[] { list });

            return this.RedirectToAction("Index");
        }
    }
}