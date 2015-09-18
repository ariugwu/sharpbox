using sharpbox.WebLibrary.Core.Strategies;
using sharpbox.WebLibrary.Data;

namespace sharpbox.WebLibrary.Core
{
    public class DefaultMediator<T> : IMediator<T>
    {
        #region Constructor(s)

      public DefaultMediator(WebContext<T> webContext, IDispatchStrategy<T> dispatchStrategy)
        {
            this.DispatchStrategy = dispatchStrategy;
            this.DispatchStrategy.RegisterCommands(webContext);
            this.DispatchStrategy.RegisterCommands(webContext);

        }

      public DefaultMediator(WebContext<T> webContext)
            : this(webContext, new DefaultDispatchStrategy<T>(webContext))
        {
            
        }

      public DefaultMediator(WebContext<T> webContext, IUnitOfWork<T> unitOfwork)
        : this(webContext, new DefaultDispatchStrategy<T>(webContext, unitOfwork))
      {
        
      } 
         
        #endregion

        #region Properties

        public IDispatchStrategy<T> DispatchStrategy { get; set; }

        #endregion


    }
}
