namespace sharpbox.WebLibrary.Core
{
    using System.Web.Mvc;

    public abstract class LifecycleHandler<T>
    {
        protected LifecycleHandler<T> _successor;

        /// <summary>
        /// The set successor.
        /// </summary>
        /// <param name="successor">
        /// The successor.
        /// </param>
        public void SetSuccessor(LifecycleHandler<T> successor)
        {
            this._successor = successor;
        }

        public void ProcessRequest(WebContext<T> webContext, Controller controller)
        {
            this.HandleRequest(webContext, controller);

            if(this._successor != null) { this._successor.ProcessRequest(webContext, controller);} 
        }

        public abstract void HandleRequest(WebContext<T> webContext, Controller controller);
    }
}
