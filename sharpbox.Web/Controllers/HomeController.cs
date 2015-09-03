using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace sharpbox.Web.Controllers
{
    using FluentValidation;

    using sharpbox.Web.Sharpbox.Core;
    using sharpbox.Web.Sharpbox.Web.Controllers;
    using sharpbox.Web.Sharpbox.Web.Helpers;

    public class HomeController : SharpboxController<string>
    {

        public HomeController()
            : base(new EmptyAppContext())
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        public override AbstractValidator<string> LoadValidatorByUiAction(UiAction uiAction)
        {
            return new InlineValidator<string>();
        }
    }
}
