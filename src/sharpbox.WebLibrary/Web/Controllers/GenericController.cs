
namespace sharpbox.WebLibrary.Web.Controllers
{
    public class GenericController<T> : SharpboxController<T> where T : class, new()
    {
    }
}
