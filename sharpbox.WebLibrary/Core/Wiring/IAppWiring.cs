namespace sharpbox.WebLibrary.Core.Wiring
{
    public interface IAppWiring
    {
        IAppPersistence AppPersistence { get; set; }

        void WireDefaultRoutes<T>(WebLibrary.Web.Controllers.ISharpboxScaffoldController<T> controller) where T : new();

        void WireContext<T>(WebLibrary.Web.Controllers.ISharpboxScaffoldController<T> controller) where T : new();

    }
}