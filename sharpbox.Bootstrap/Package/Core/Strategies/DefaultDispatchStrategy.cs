using sharpbox.Bootstrap.Package.Core.Strategies;
using sharpbox.WebLibrary.Data;

namespace sharpbox.WebLibrary.Core.Strategies
{

  public class DefaultDispatchStrategy<T> : BaseDispatchStrategy<T>
  {

    public DefaultDispatchStrategy(WebContext<T> webContext, IUnitOfWork<T> unitOfWork) : base(webContext, unitOfWork)
    {
    }

    public DefaultDispatchStrategy(WebContext<T> webContext)
      : this(webContext, new DefaultUnitOfWork<T>())
    {
    }

  }
}
