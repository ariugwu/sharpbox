using sharpbox.Dispatch.Model;
using sharpbox.Web.Sharpbox.Core.Strategies;
using sharpbox.Web.Sharpbox.Data;

namespace sharpbox.Web.Sharpbox.Core
{
    public class DefaultMediator<T> : IMediator<T>
    {
        #region Constructor(s)

        public DefaultMediator(AppContext appContext, IDispatchStrategy<T> dispatchStrategy, IUnitOfWork<T> unitOfWork)
        {
            this.DispatchStrategy = dispatchStrategy;

            RegisterCommands(appContext);
            RegisterListeners(appContext);
        }

        public DefaultMediator(AppContext appContext, IDispatchStrategy<T> dispatchStrategy)
            : this(appContext, dispatchStrategy, new DefaultUnitOfWork<T>())
        {
            
        }

        public DefaultMediator(AppContext appContext)
            : this(appContext, new DefaultDispatchStrategy<T>(), new DefaultUnitOfWork<T>())
        {
            
        }
         
        #endregion

        #region Properties

        public IDispatchStrategy<T> DispatchStrategy { get; set; }

        #endregion

        #region Interface Methods

        public Feedback<T> Process(AppContext appContext, T instance, CommandNames commandName)
        {
            return this.DispatchStrategy.Process(appContext, instance, commandName);
        }

        public void RegisterCommands(AppContext appContext)
        {
            this.DispatchStrategy.RegisterCommands(appContext);
        }

        public void RegisterListeners(AppContext appContext)
        {
            this.DispatchStrategy.RegisterCommands(appContext);
        }

        #endregion

    }
}
