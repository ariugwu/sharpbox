using sharpbox.Bootstrap.Package.Core.Strategies;
using sharpbox.WebLibrary.Data;

namespace sharpbox.WebLibrary.Core.Strategies
{

  public class DefaultMediator<T> : BaseMediator<T> where T : new()
  {

    public DefaultMediator(AppContext appContext, IUnitOfWork<T> unitOfWork) : base(appContext, unitOfWork)
    {
    }

    public DefaultMediator(AppContext appContext)
      : this(appContext, new DefaultUnitOfWork<T>())
    {
    }

      public override void RegisterCommands(WebContext<T> webContext)
      {
          // Intentionally left blank
      }

      public override void RegisterListeners(WebContext<T> webContext)
      {
          // Intentionally left blank
      }
  }
}
