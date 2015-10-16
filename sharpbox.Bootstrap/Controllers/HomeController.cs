using System.Linq;
using System.Web.Mvc;
using sharpbox.WebLibrary.Helpers.TypeScript;

namespace sharpbox.Bootstrap.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var test = new FluxStore();

            var names = MetaLoader.GetAllMetaDataClasses(); //test.Wat();

            var fields = names.SelectMany(x => x.GetFields()).ToList();

            var hmm = fields.SelectMany(x => x.Name);

            var woo = names.SelectMany(x => x.Name);

            var blerg = fields.ToArray();

            var arg = hmm.ToList();

            var clerg = woo.ToList();

            return View();
        }
    }
}
