using sharpbox.WebLibrary.Core.Strategies;

namespace sharpbox.WebLibrary.Core
{
    public class DefaultMediator<T> : IMediator<T>
    {
        #region Constructor(s)

      public DefaultMediator(WebContext<T> webContext, IDispatchStrategy<T> dispatchStrategy)
        {
            this.DispatchStrategy = dispatchStrategy;

            RegisterCommands(webContext);
            RegisterListeners(webContext);
        }

      public DefaultMediator(WebContext<T> webContext)
            : this(webContext, new DefaultDispatchStrategy<T>())
        {
            
        }
         
        #endregion

        #region Properties

        public IDispatchStrategy<T> DispatchStrategy { get; set; }

        #endregion

        #region Interface Methods

        public void RegisterCommands(WebContext<T> webContext)
        {
            this.DispatchStrategy.RegisterCommands(webContext);
        }

        public void RegisterListeners(WebContext<T> webContext)
        {
            this.DispatchStrategy.RegisterCommands(webContext);
        }

        #endregion

    }
}
