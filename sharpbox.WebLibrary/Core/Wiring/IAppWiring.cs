namespace sharpbox.WebLibrary.Core.Wiring
{
    public interface IAppWiring
    {
        IAppPersistence AppPersistence { get; set; }

        void WireDefaultRoutes<T>(WebLibrary.Web.Controllers.ISharpboxController<T> controller) where T : class, new();

        void WireContext<T>(WebLibrary.Web.Controllers.ISharpboxController<T> controller) where T : class, new();

    }
}